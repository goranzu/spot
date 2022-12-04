using MySpot.Application.Services;
using MySpot.Core.Abstractions;

namespace MySpot.Infrastructure.Time;

public sealed class ClockService : IClockService
{
    public DateTime Current() => DateTime.UtcNow;
}