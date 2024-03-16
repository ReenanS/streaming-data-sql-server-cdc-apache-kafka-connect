using Confluent.Kafka;
using Poc.Debezium.Kafka.Consumer;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        const string table = "dbo.Pessoa";
        const string serverName = "watcher-pessoa";
        const string topic = $"{serverName}.{table}";
        var consumerConfig = context.Configuration.GetSection("ConsumerConfig").Get<ConsumerConfig>();
        var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        consumer.Subscribe(topic);
        services.AddSingleton(_ => consumer);
        services.AddHostedService<Worker>();
    }).Build();

await host.RunAsync();
