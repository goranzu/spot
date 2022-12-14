using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public sealed class WeeklyParkingSpot
{
    public ParkingSpotId Id { get; }
    public Week Week { get; private set; }
    public ParkingSpotName Name { get; private set; }
    public IEnumerable<Reservation> Reservations => _reservations;

    private readonly HashSet<Reservation> _reservations = new();

    public WeeklyParkingSpot(ParkingSpotId id, Week week, ParkingSpotName name)
    {
        Id = id;
        Week = week;
        Name = name;
    }

    internal void AddReservation(Reservation reservation, Date now)
    {
        var isInvalidDate = reservation.Date < Week.From || reservation.Date > Week.To ||
                            reservation.Date < now;

        if (isInvalidDate)
        {
            throw new InvalidReservationDateException(reservation.Date.Value.Date);
        }

        var alreadyReserved = _reservations.Any(x => x.Date == reservation.Date);

        if (alreadyReserved)
        {
            throw new ParkingSpotAlreadyReservedException(Name, reservation.Date.Value.Date);
        }

        _reservations.Add(reservation);
    }

    public void DeleteReservation(ReservationId id)
    {
        _reservations.RemoveWhere(x => x.Id == id);
    }
}