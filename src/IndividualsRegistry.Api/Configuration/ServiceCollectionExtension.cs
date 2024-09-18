using IndividualsRegistry.Domain.Settings;

namespace IndividualsRegistry.Api.Configuration;

internal static class ServiceCollectionExtension
{
    public static void AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
    }
}
