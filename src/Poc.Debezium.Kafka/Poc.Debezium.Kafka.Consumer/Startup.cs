using Amazon.SecretsManager;
using Amazon.StepFunctions;
using Domain.Configurations;
using Infra.Configurations;
using Kafka.Configuration;
using Kafka.Services;
using Worker.BackgroundTask;
using Worker.Configurations;
using Worker.Extensions;
using Worker.Services;
using LocalStack.Client.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using System;
using System.Diagnostics.CodeAnalysis;

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

            Log.Logger = new ILoggerProviderConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.WithExceptionDetails()
                .WriteTo.Console(new JsonFormatter())
                .CreateLogger();

            services.AddLocalStack(Configuration);
            services.AddSingleton(Log.Logger);

            services.AddInfra()
                .AddKafka(Configuration)
            .AddWorker()
            .AddDomain();

            services.AddAwsService<IAmazonSecretsManager>();
            services.AddAwsService<IAmazonStepFunction>();

            services.AddHostedService<KCertGeneratorService>();
            services.AddHostedService<KafkaConsumerService>();
            services.AddHostedService<QueuedHostedService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies);
            services.AddHealthCheckService();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.RegisterHealthCheck();
        }
    }
}
