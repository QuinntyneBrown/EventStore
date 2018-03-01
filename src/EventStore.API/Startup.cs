using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using EventStore.Web.Services;
using EventStore.API.Services;
using EventStore.API.Extensions;
using EventStore.Infrastructure.Services;
using EventStore.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace EventStore.Web
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
            services.AddSecurity(Configuration);
            services.AddCustomConfiguration(Configuration);
            services.AddHttpClient();
            services.AddDataStores(Configuration["Data:DefaultConnection:ConnectionString"]);
            services.AddCustomSwagger();
            services.AddCustomMediator();
            services.AddCustomCache();
            services.AddSignalR();
            services.AddCustomMvc();
        }

        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, IEncryptionService encryptionService, EventStoreContext context, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseSecurity();
            app.UseMvc();
            app.UseSignalR(routes => routes.MapHub<EventStoreHub>("/appHub"));
            app.UseCustomSwagger();
            
        }
    }
}
