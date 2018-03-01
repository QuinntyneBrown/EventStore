using Microsoft.AspNetCore.Builder;

namespace EventStore.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventStore API V1"));
            return app;
        }

        public static IApplicationBuilder UseSecurity(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseCors("CorsPolicy");
            return app;
        }
    }
}
