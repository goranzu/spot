using MySpot.Core.Abstractions;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Policies;

internal sealed class RegularEmployeeReservationPolicy : IReservationPolicy
{
    private readonly IClockService _clockService;

    public RegularEmployeeReservationPolicy(IClockService clockService)
    {
        _clockService = clockService;
    }

    public bool CanBeApplied(JobTitle jobTitle)
        => jobTitle.Value == JobTitle.Employee;

    public bool CanReserve(IEnumerable<WeeklyParkingSpot> weeklyParkingSpots, EmployeeName employeeName)
    {
        var totalEmployeeReservations = weeklyParkingSpots
            .SelectMany(x => x.Reservations)
            .Count(x => x.EmployeeName == employeeName);

        return totalEmployeeReservations <= 2 && _clockService.Current().Hour > 4;
    }
}