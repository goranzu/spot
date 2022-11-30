using MySpot.Application.Commands;
using MySpot.Application.Services;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.DataAccess.Repositories;
using MySpot.Tests.Unit.Shared;
using Shouldly;

namespace MySpot.Tests.Unit.Services;

public class ReservationServiceTests
{
    [Fact]
    public void given_valid_command_create_should_add_reservation()
    {
        var command = new CreateReservation(Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.NewGuid(),
            "John Doe", "XYZ123", DateTime.UtcNow.AddDays(1));

        var reservationId = _reservationService.Create(command);

        reservationId.ShouldNotBeNull();
        reservationId.Value.ShouldBe(command.ReservationId);
    }

    [Fact]
    public void given_invalid_parkingspot_id_create_should_fail()
    {
        var command = new CreateReservation(Guid.Parse("00000000-0000-0000-0000-000000000011"), Guid.NewGuid(),
            "John Doe", "XYZ123", DateTime.UtcNow.AddDays(1));

        var reservationId = _reservationService.Create(command);

        reservationId.ShouldBeNull();
    }

    [Fact]
    public void given_reservation_taken_date_reservation_should_fail()
    {
        var command = new CreateReservation(Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.NewGuid(),
            "John Doe", "XYZ123", DateTime.UtcNow.AddDays(1));
        _reservationService.Create(command);

        var reservationId = _reservationService.Create(command);

        reservationId.ShouldBeNull();
    }

    #region ARRANGE

    private readonly ReservationService _reservationService;
    private readonly IClockService _clock;
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;

    public ReservationServiceTests()
    {
        _clock = new TestClockService();
        _weeklyParkingSpotRepository = new InMemoryWeeklyParkingSpotRepository(_clock);
        _reservationService = new ReservationService(_clock, _weeklyParkingSpotRepository);
    }

    #endregion
}