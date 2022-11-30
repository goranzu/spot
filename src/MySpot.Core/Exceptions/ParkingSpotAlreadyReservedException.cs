namespace MySpot.Core.Exceptions;

public sealed class ParkingSpotAlreadyReservedException : CustomException
{
    public string ParkingSpotName { get; }
    public DateTime Date { get; }

    public ParkingSpotAlreadyReservedException(string parkingSpotName, DateTime date) 
        : base($"Parking spot with name {parkingSpotName} is already reserved on {date.Date}")
    {
        ParkingSpotName = parkingSpotName;
        Date = date;
    }
}