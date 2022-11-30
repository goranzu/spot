using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.DataAccess.Repositories;

namespace MySpot.Infrastructure.DataAccess;

internal static class DataAccessExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        const string connectionString = "Host=localhost;Database=myspot;Username=postgres;Password=";
        services.AddDbContext<MySpotDbContext>(x => x.UseNpgsql(connectionString));

        services.AddScoped<IWeeklyParkingSpotRepository, PostgresWeeklyParkingSpotRepository>();
        return services;
    }
}