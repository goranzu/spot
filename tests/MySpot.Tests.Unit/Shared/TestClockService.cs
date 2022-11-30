using MySpot.Application.Services;

namespace MySpot.Tests.Unit.Shared;

internal sealed class TestClockService : IClockService
{
    public DateTime Current() => new DateTime(2022,11, 28);
}