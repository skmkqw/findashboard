using ZBank.API;
using ZBank.API.Hubs;
using ZBank.Application;
using ZBank.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentation()
        .AddApplication()
        .AddInfrastructure(builder.Configuration); 
}

var app = builder.Build();
{
    app.UseExceptionHandler("/error");

    app.UseCors("AllowFrontend");
    
    app.UseAuthentication();
    
    app.UseHttpsRedirection();
    
    app.UseAuthorization();

    app.MapControllers();
    
    app.MapHub<NotificationHub>("/notification-hub");

    app.Run();
}