using EventStore.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace EventStore.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {        
        public static IMvcBuilder AddCustomMvc(this IServiceCollection services)
        {
            return services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            }).AddControllersAsServices();            
        }

        public static IServiceCollection AddCustomSignalR(this IServiceCollection services)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new SignalRContractResolver()
            };

            var serializer = JsonSerializer.Create(settings);

            services.Add(new ServiceDescriptor(typeof(JsonSerializer),
                                               provider => serializer,
                                               ServiceLifetime.Transient));
            services.AddSignalR();

            return services;
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "EventStore",
                    Version = "v1",
                    Description = "EventStore REST API",
                });
                options.CustomSchemaIds(x => x.FullName);
            });

            
            return services;
        }        
    }
}
