using Confluent.Kafka;
using Poc.Debezium.Kafka.Consumer;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        const string table = "dbo.CONBE007";
        const string serverName = "cdc-sqlserver";
        //const string topic = $"{serverName}.{table}";
        const string topic = $"joaozinho.DBCN502.dbo.CONBE007";
        var consumerConfig = context.Configuration.GetSection("ConsumerConfig").Get<ConsumerConfig>();
        var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        consumer.Subscribe(topic);
        services.AddSingleton(_ => consumer);
        services.AddHostedService<Worker>();
    }).Build();

await host.RunAsync();
