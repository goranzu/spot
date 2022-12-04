﻿using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.DomainServices;

public interface IParkingReservationService
{
    public void ReserveSpotForVehicle(IEnumerable<WeeklyParkingSpot> weeklyParkingSpots, JobTitle jobTitle,
        WeeklyParkingSpot parkingSpotToReserve, Reservation reservation);
}