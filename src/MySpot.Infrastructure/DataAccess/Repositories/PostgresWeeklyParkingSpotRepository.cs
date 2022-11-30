using Microsoft.EntityFrameworkCore;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DataAccess.Repositories;

internal sealed class PostgresWeeklyParkingSpotRepository : IWeeklyParkingSpotRepository
{
    private readonly MySpotDbContext _dbContext;
    private readonly DbSet<WeeklyParkingSpot> _weeklyParkingSpots;

    public PostgresWeeklyParkingSpotRepository(MySpotDbContext dbContext)
    {
        _dbContext = dbContext;
        _weeklyParkingSpots = _dbContext.WeeklyParkingSpots;
    }

    public IEnumerable<WeeklyParkingSpot> GetAll()
        => _weeklyParkingSpots
            .Include(x => x.Reservations)
            .ToList();

    public IEnumerable<WeeklyParkingSpot> GetByWeek(Week week)
        => _weeklyParkingSpots
            .Where(x => x.Week == week)
            .Include(x => x.Reservations)
            .ToList();

    public WeeklyParkingSpot? Get(ParkingSpotId id) => _weeklyParkingSpots
        .Include(x => x.Reservations)
        .SingleOrDefault(x => x.Id == id);

    public void Add(WeeklyParkingSpot weeklyParkingSpot)
    {
        _weeklyParkingSpots.Add(weeklyParkingSpot);
        _dbContext.SaveChanges();
    }

    public void Update(WeeklyParkingSpot weeklyParkingSpot)
    {
        _weeklyParkingSpots.Update(weeklyParkingSpot);
        _dbContext.SaveChanges();
    }
}