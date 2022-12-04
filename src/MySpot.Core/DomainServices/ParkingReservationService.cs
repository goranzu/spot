using MySpot.Core.Abstractions;
using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.Policies;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.DomainServices;

class ParkingReservationService : IParkingReservationService
{
    private readonly IEnumerable<IReservationPolicy> _reservationPolicies;
    private readonly IClockService _clockService;

    public ParkingReservationService(IEnumerable<IReservationPolicy> reservationPolicies, IClockService clockService)
    {
        _reservationPolicies = reservationPolicies;
        _clockService = clockService;
    }

    public void ReserveSpotForVehicle(IEnumerable<WeeklyParkingSpot> weeklyParkingSpots, JobTitle jobTitle,
        WeeklyParkingSpot parkingSpotToReserve, Reservation reservation)
    {
        var policy = _reservationPolicies
            .SingleOrDefault(x => x.CanBeApplied(jobTitle));
        if (policy is null)
        {
            throw new NoReservationPolicyFoundException(jobTitle.Value);
        }

        if (!policy.CanReserve(weeklyParkingSpots, reservation.EmployeeName))
        {
            throw new CannotReserveParkingSpotException(parkingSpotToReserve.Id);
        }

        parkingSpotToReserve.AddReservation(reservation, new Date(_clockService.Current()));
    }
}