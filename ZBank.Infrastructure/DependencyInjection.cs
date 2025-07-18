using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Interfaces.Services;
using ZBank.Application.Common.Interfaces.Services.Authentication;
using ZBank.Infrastructure.Authentication;
using ZBank.Infrastructure.Persistance;
using ZBank.Infrastructure.Persistance.Repositories;
using ZBank.Infrastructure.Services;
using ZBank.Infrastructure.Services.Background.Currencies;
using ZBank.Infrastructure.Services.Background.Wallets;

namespace ZBank.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAuth(configuration);
        
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IUserConnectionManager, UserConnectionManager>();
        services.AddSingleton<IGroupManager, GroupManager>();


        services.AddHostedService<CurrencyUpdaterService>();
        services.AddHostedService<CurrencyUpdateSenderService>();
        
        services.AddHostedService<WalletUpdaterService>();
        services.AddHostedService<WalletUpdateSenderService>();
        
        services.AddPersistence();
        return services;
    }
    
    private static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<ZBankDbContext>(options => options.UseNpgsql(connectionString: ""));
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);
        
        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
                
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.TryGetValue("AuthToken", out var token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }
}
