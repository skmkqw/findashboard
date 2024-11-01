using System.Reflection;
using Mapster;
using MapsterMapper;

namespace ZBank.API.Mapping;

public static class DependencyInjection
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
        
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        
        return services;
    }
}