using Microsoft.EntityFrameworkCore;
using MySpot.Core.Entities;

namespace MySpot.Infrastructure.DataAccess;

internal sealed class MySpotDbContext : DbContext
{
    public DbSet<Reservation> Reservations { get; set; } = null!;
    public DbSet<WeeklyParkingSpot> WeeklyParkingSpots { get; set; } = null!;

    public MySpotDbContext(DbContextOptions<MySpotDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}