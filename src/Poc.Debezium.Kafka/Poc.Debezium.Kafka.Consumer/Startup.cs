using Amazon.StepFunctions;
using Application.Configurations;
using Confluent.Kafka;
using Domain.Interfaces;
using Infra.Configurations;
using Infra.Services;
using LocalStack.Client.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Worker.BackgroundTask;
using Worker.Configurations;

namespace Worker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "LocalStartup";

            Console.WriteLine(environment);

            //Log.Logger = new ILoggerProviderConfiguration()
            //    .ReadFrom.Configuration(Configuration)
            //    .Enrich.WithExceptionDetails()
            //    .WriteTo.Console(new JsonFormatter())
            //    .CreateLogger();

            services.AddLocalStack(Configuration);
            services.AddSingleton(Log.Logger);

            services.AddInfra()
                .AddKafka(Configuration)
                .AddWorker()
                .AddApplication();

            // Configuração do Kafka Consumer
            services.AddSingleton<IConsumer<string, string>>(provider =>
            {
                var config = new ConsumerConfig
                {
                    BootstrapServers = Configuration["Kafka:BootstrapServers"],
                    GroupId = Configuration["Kafka:GroupId"],
                };

                return new ConsumerBuilder<string, string>(config).Build();
            });

            //services.AddAwsService<IAmazonSecretsManager>();
            services.AddAwsService<IAmazonStepFunctions>();

            // Registrar o KafkaConsumerManager
            //services.AddScoped<KafkaConsumerManager>();

            //services.AddHostedService<KCertGeneratorService>();
            services.AddHostedService<KafkaConsumerService>();
            services.AddHostedService<QueuedHostedService>();

            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies);
            //services.AddHealthCheckService();

            services.AddSingleton<IMessageProcessor, MessageProcessor>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.RegisterHealthCheck();

        }
    }
}
