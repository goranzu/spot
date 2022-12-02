using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySpot.Application.Services;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DataAccess;

internal sealed class DatabaseInitializer : IHostedService
{
    // service locator "anti-pattern" this is the DI container
    private readonly IServiceProvider _serviceProvider;
    private readonly IClockService _clockService;

    public DatabaseInitializer(IServiceProvider serviceProvider, IClockService clockService)
    {
        _serviceProvider = serviceProvider;
        _clockService = clockService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MySpotDbContext>();
        await dbContext.Database.MigrateAsync(cancellationToken);

        if (await dbContext.WeeklyParkingSpots.AnyAsync(cancellationToken))
        {
            return;
        }

        await dbContext.WeeklyParkingSpots.AddRangeAsync(
            new List<WeeklyParkingSpot>
            {
                new(Guid.Parse("00000000-0000-0000-0000-000000000001"), new Week(_clockService.Current()), "P1"),
                new(Guid.Parse("00000000-0000-0000-0000-000000000002"), new Week(_clockService.Current()), "P2"),
                new(Guid.Parse("00000000-0000-0000-0000-000000000003"), new Week(_clockService.Current()), "P3"),
                new(Guid.Parse("00000000-0000-0000-0000-000000000004"), new Week(_clockService.Current()), "P4"),
                new(Guid.Parse("00000000-0000-0000-0000-000000000005"), new Week(_clockService.Current()), "P5"),
            }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}