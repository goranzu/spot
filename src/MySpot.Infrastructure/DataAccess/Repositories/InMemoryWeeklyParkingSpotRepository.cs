using MySpot.Application.Services;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DataAccess.Repositories;

public sealed class InMemoryWeeklyParkingSpotRepository : IWeeklyParkingSpotRepository
{
    private readonly List<WeeklyParkingSpot> _weeklyParkingSpots;

    public InMemoryWeeklyParkingSpotRepository(IClockService clockService)
    {
        _weeklyParkingSpots = new List<WeeklyParkingSpot>
        {
            new(Guid.Parse("00000000-0000-0000-0000-000000000001"), new Week(clockService.Current()), "P1"),
            new(Guid.Parse("00000000-0000-0000-0000-000000000002"), new Week(clockService.Current()), "P2"),
            new(Guid.Parse("00000000-0000-0000-0000-000000000003"), new Week(clockService.Current()), "P3"),
            new(Guid.Parse("00000000-0000-0000-0000-000000000004"), new Week(clockService.Current()), "P4"),
            new(Guid.Parse("00000000-0000-0000-0000-000000000005"), new Week(clockService.Current()), "P5"),
        };
    }

    public IEnumerable<WeeklyParkingSpot> GetAll()
    {
        return _weeklyParkingSpots;
    }

    public WeeklyParkingSpot? Get(ParkingSpotId id)
    {
        return _weeklyParkingSpots.SingleOrDefault(x => x.Id == id);
    }

    public void Add(WeeklyParkingSpot weeklyParkingSpot)
    {
        _weeklyParkingSpots.Add(weeklyParkingSpot);
    }

    public void Update(WeeklyParkingSpot weeklyParkingSpot)
    {
        throw new NotImplementedException();
    }
}