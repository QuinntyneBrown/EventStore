using EventStore.Core.Interfaces;
using EventStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EventStore.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {                
        public static IServiceCollection AddDataStore(this IServiceCollection services,
                                               string connectionString, bool useInMemoryDatabase = false)
        {
            services.AddScoped<IAppDbContext, AppDbContext>();

            if (useInMemoryDatabase) {
                services.AddDbContext<AppDbContext>(options =>
                {
                    options
                    .UseLoggerFactory(AppDbContext.ConsoleLoggerFactory)
                    .UseInMemoryDatabase(databaseName: $"InMemoryDatabase");
                });

                return services;
            }
            services.AddDbContext<AppDbContext>(options =>
            {                
                options
                .UseLoggerFactory(AppDbContext.ConsoleLoggerFactory)
                .UseSqlServer(connectionString, b=> b.MigrationsAssembly("EventStore.Infrastructure"));
            });

            return services;
        }
    }
}
