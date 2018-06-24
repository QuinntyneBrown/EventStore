using EventStore.Core.Behaviours;
using EventStore.Core.Extensions;
using EventStore.Core.Interfaces;
using EventStore.Infrastructure.Data;
using EventStore.Infrastructure.Extensions;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EventStore.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder().Build();
            ProcessDbCommands(args, host);
            host.Run();
        }
        
        public static IWebHostBuilder CreateWebHostBuilder() =>
            WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>();

        private static void ProcessDbCommands(string[] args, IWebHost host)
        {
            var services = (IServiceScopeFactory)host.Services.GetService(typeof(IServiceScopeFactory));

            using (var scope = services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (args.Contains("ci"))
                    args = new string[4] { "dropdb", "migratedb", "seeddb", "stop" };

                if (args.Contains("dropdb"))
                    context.Database.EnsureDeleted();

                if (args.Contains("migratedb"))
                    context.Database.Migrate();

                if (args.Contains("seeddb"))
                {
                    context.Database.EnsureCreated();
                    SeedData.Seed(context);            
                }
                
                if (args.Contains("stop"))
                    Environment.Exit(0);
            }
        }        
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc()
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });

            services.AddDistributedMemoryCache()
                .AddDataStore(Configuration["Data:DefaultConnection:ConnectionString"], Configuration.GetValue<bool>("isTest"))
                .AddCustomSignalR()
                .AddCustomSwagger()
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
                .AddMediatR(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IAppDbContext context)
        {
            app.UseCors("CorsPolicy")
                .UseMvc()
                .UseSwagger()
                .UseSwaggerUI(options
                => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Note Taking App API"));            
        }
    }
}
