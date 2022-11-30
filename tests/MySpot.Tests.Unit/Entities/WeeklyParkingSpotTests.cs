using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;
using Shouldly;

namespace MySpot.Tests.Unit.Entities;

public class WeeklyParkingSpotTests
{
    [Theory]
    [InlineData("2020-02-02")]
    [InlineData("2025-02-02")]
    [InlineData("2022-11-27")]
    public void given_invalid_date_add_reservation_should_fail(string dateString)
    {
        var invalidDate = DateTime.Parse(dateString);

        var reservation = new Reservation(Guid.NewGuid(), "John Doe", "XYZ123",
            new Date(invalidDate));

        var exception = Record.Exception(() => _weeklyParkingSpot.AddReservation(reservation, _now));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidReservationDateException>();
    }

    [Fact]
    public void given_reservation_for_already_existing_date_add_reservation_should_fail()
    {
        var reservationDate = _now.AddDays(1);
        var reservation = new Reservation(Guid.NewGuid(), "John Doe", "XYZ123",
            reservationDate);
        _weeklyParkingSpot.AddReservation(reservation, reservationDate);


        var exception = Record.Exception(() => _weeklyParkingSpot.AddReservation(reservation, reservationDate));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ParkingSpotAlreadyReservedException>();
    }

    [Fact]
    public void given_reservation_for_not_already_existing_date_add_reservation_should_succeed()
    {
        var reservationDate = _now.AddDays(1);
        var reservation = new Reservation(Guid.NewGuid(), "John Doe", "XYZ123",
            reservationDate);

        _weeklyParkingSpot.AddReservation(reservation, reservationDate);
        
        _weeklyParkingSpot.Reservations.ShouldHaveSingleItem();
        _weeklyParkingSpot.Reservations.ShouldContain(reservation);
    }

    #region ARRANGE

    private readonly WeeklyParkingSpot _weeklyParkingSpot;
    private readonly Date _now;

    public WeeklyParkingSpotTests()
    {
        _now = new Date(DateTime.Parse("2022-11-28"));
        _weeklyParkingSpot = new WeeklyParkingSpot(Guid.NewGuid(), new Week(_now), "P1");
    }

    #endregion
}