namespace Api.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Services;

    public static class ServiceExtensions
    {
        public static void ConfigureIntegrationServices(this IServiceCollection services)
        {
            services.AddHttpClient<IIntegrationService, IntegrationService>();
        }
    }
}