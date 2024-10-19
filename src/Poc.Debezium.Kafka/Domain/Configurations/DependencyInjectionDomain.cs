﻿using Domain.Interfaces.UseCases;
using Domain.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Configurations
{
    public static class DependencyInjectionDomain
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddScoped<ISendOperationToStepFunction, SendOperationToStepFunction>();
            return services;
        }
    }
}
