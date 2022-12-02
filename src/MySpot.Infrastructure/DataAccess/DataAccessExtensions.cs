using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.DataAccess.Repositories;

namespace MySpot.Infrastructure.DataAccess;

internal static class DataAccessExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        // const string connectionString = "Host=localhost;Database=myspot;Username=postgres;Password=";
        //
        // services.AddDbContext<MySpotDbContext>(x => x.UseNpgsql(connectionString));

        services.Configure<PostgresOptions>(configuration.GetRequiredSection(PostgresOptions.SectionName));
        var postgresOptions = configuration.GetOptions<PostgresOptions>(PostgresOptions.SectionName);
        services.AddDbContext<MySpotDbContext>(x => x.UseNpgsql(postgresOptions.ConnectionString));

        services.AddScoped<IWeeklyParkingSpotRepository, PostgresWeeklyParkingSpotRepository>();

        services.AddHostedService<DatabaseInitializer>();

        // FIX EF Core Errors
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        return services;
    }
}