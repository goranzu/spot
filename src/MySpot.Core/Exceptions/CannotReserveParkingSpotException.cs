using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions;

public class CannotReserveParkingSpotException : CustomException
{
    public ParkingSpotId Id { get; }

    public CannotReserveParkingSpotException(ParkingSpotId id) 
        : base($"Cannot reserve parking spot with ID '{id}'.")
    {
        Id = id;
    }
}