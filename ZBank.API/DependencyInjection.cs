using ZBank.API.Hubs;
using ZBank.API.Interfaces;
using ZBank.API.Mapping;
using ZBank.API.Services.Notifications;

namespace ZBank.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddMappers();
        services.AddSignalR();
        
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        services.AddScoped<INotificationSender, NotificationSender<NotificationHub>>();
        
        return services;
    }
}