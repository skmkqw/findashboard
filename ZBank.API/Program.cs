using ZBank.API;
using ZBank.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentation()
        .AddInfrastructure(builder.Configuration); 
}

var app = builder.Build();
{
    app.UseExceptionHandler("/error");

    app.UseAuthentication();
    
    app.UseHttpsRedirection();
    
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}