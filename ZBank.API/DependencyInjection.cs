using ZBank.API.Mapping;

namespace ZBank.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddMappers();
        return services;
    }
}