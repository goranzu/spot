using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Services;
using MySpot.Core.Abstractions;
using MySpot.Infrastructure.DataAccess;
using MySpot.Infrastructure.Exceptions;
using MySpot.Infrastructure.Time;

namespace MySpot.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // var appOptions = configuration.GetOptions<AppOptions>("app");
        // services.Configure<AppOptions>(configuration.GetRequiredSection("app"));
        services.AddSingleton<ExceptionMiddleware>();
        services.AddControllers();

        services
            .AddDataAccess(configuration)
            // .AddSingleton<IWeeklyParkingSpotRepository, InMemoryWeeklyParkingSpotRepository>()
            .AddSingleton<IClockService, ClockService>();

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        app.MapControllers();
        return app;
    }

    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        var section = configuration.GetRequiredSection(sectionName);
        section.Bind(options);
        return options;
    }
}