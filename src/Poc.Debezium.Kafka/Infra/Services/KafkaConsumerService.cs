// src/Infrastructure/Services/KafkaConsumerService.cs
using Application.Commands; // Namespace para o comando
using Confluent.Kafka;
using Domain.Interfaces.BackgroundTask;
using Infra.Configurations;
using Kafka.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infra.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceProvider;
        private readonly KafkaConfiguration _kafkaConfiguration;
        private readonly ILogger<KafkaConsumerService> _logger;
        private readonly IBackgroundTaskQueue _taskQueue; // Injeta o Task Queue

        public KafkaConsumerService(
            IServiceScopeFactory serviceProvider,
            IOptions<KafkaConfiguration> kafkaConfiguration,
            ILogger<KafkaConsumerService> logger,
            IBackgroundTaskQueue taskQueue)
        {
            _serviceProvider = serviceProvider;
            _kafkaConfiguration = kafkaConfiguration.Value;
            _logger = logger;
            _taskQueue = taskQueue; // Atribui TaskQueue
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            // Criar um consumidor para cada tópico na configuração
            foreach (var topicConfig in _kafkaConfiguration.Topics)
            {
                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = _kafkaConfiguration.BootstrapServers,
                    GroupId = topicConfig.GroupId,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = false // Desativa AutoCommit
                };

                // Iniciar a tarefa de consumo para cada tópico
                _ = Task.Run(() => ConsumeFromTopic(consumerConfig, topicConfig.Topic, stoppingToken), stoppingToken);
            }
        }

        private async Task ConsumeFromTopic(ConsumerConfig config, string topic, CancellationToken stoppingToken)
        {
            using var consumer = new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe(topic);

            int messageCount = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);

                    // Verificação de nulidade
                    if (consumeResult?.Message == null || string.IsNullOrEmpty(consumeResult.Message.Value))
                    {
                        _logger.LogWarning($"Mensagem vazia ou nula recebida do tópico {topic}, ignorando...");
                        continue;
                    }

                    messageCount++;

                    _logger.LogWarning($"Mensagem {messageCount} recebida do tópico {topic}");

                    // Adicionar o trabalho à fila de tarefas
                    _taskQueue.QueueBackgroundWorkItem(async (token) =>
                    {
                        await ProcessKafkaMessage(consumeResult.Message.Value, topic, consumeResult, consumer);
                    });
                }
                catch (ConsumeException e)
                {
                    _logger.LogError($"Erro ao consumir mensagem do tópico {topic}: {e.Error.Reason}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Erro inesperado: {ex.Message}");
                }
            }
        }

        private async Task ProcessKafkaMessage(string messageValue, string topic, ConsumeResult<string, string> consumeResult, IConsumer<string, string> consumer)
        {
            // Deserializar a mensagem Kafka
            var kafkaMessage = JsonConvert.DeserializeObject<KafkaMessage>(messageValue);
            if (kafkaMessage?.payload == null)
            {
                _logger.LogWarning("Payload nulo, ignorando a mensagem...");
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            var commandHandler = scope.ServiceProvider.GetRequiredService<ProcessKafkaMessageCommandHandler>();
            var command = new ProcessKafkaMessageCommand(kafkaMessage, topic);

            await commandHandler.Handle(command, default);

            // Commit manual após processar a mensagem com sucesso
            consumer.Commit(consumeResult);
        }
    }
}
