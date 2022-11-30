using MySpot.Application.Services;

namespace MySpot.Infrastructure.Time;

public sealed class ClockService : IClockService
{
    public DateTime Current() => DateTime.UtcNow;
}