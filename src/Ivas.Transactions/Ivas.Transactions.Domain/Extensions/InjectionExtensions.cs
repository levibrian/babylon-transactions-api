﻿using System.Collections.Generic;
using AutoMapper;
using Ivas.Transactions.Domain.Mappers;
using Ivas.Transactions.Domain.Services;
using Ivas.Transactions.Domain.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Ivas.Transactions.Domain.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection RegisterDomainDependencies(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .RegisterServices()
                .RegisterMappers();

        }

        private static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddTransient<ITransactionValidator, TransactionValidator>()
                .AddTransient<ITransactionService, TransactionService>()
                .AddTransient<IPortfolioService, PortfolioService>();
        }
        
        private static IServiceCollection RegisterMappers(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddAutoMapper(config => 
                    config.AddProfiles(new List<Profile>()
                    {
                        new TransactionProfile(),
                        new TransactionSummaryProfile()
                    }));
        }
    }
}