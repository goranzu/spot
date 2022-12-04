using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Policies;

public interface IReservationPolicy
{
    public bool CanBeApplied(JobTitle jobTitle);
    public bool CanReserve(IEnumerable<WeeklyParkingSpot> weeklyParkingSpots, EmployeeName employeeName);
}