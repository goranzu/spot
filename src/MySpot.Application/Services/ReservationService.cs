using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Exceptions;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Services;

public sealed class ReservationService : IReservationService
{
    private readonly IClockService _clockService;
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;


    public ReservationService(IClockService clockService, IWeeklyParkingSpotRepository weeklyParkingSpotRepository)
    {
        _clockService = clockService;
        _weeklyParkingSpotRepository = weeklyParkingSpotRepository;
    }

    public async Task<IEnumerable<ReservationDto>> GetAllWeeklyReservationsAsync()
    {
        return (await _weeklyParkingSpotRepository
                .GetAllAsync())
            .SelectMany(x => x.Reservations)
            .Select(x => new ReservationDto
            {
                Id = x.Id,
                EmployeeName = x.EmployeeName,
                Date = x.Date.Value.Date
            });
    }

    public async Task<ReservationDto?> GetAsync(Guid id)
    {
        return (await GetAllWeeklyReservationsAsync()).SingleOrDefault(x => x.Id == id);
    }

    public async Task CreateAsync(CreateReservation command)
    {
        var (spotId, reservationId, employeeName, licensePlate, date) = command;

        var parkingSpotId = new ParkingSpotId(spotId);
        var spot = await _weeklyParkingSpotRepository.GetAsync(parkingSpotId);

        if (spot is null)
        {
            throw new WeeklyParkingSpotNotFoundException(spotId);
        }

        var reservation = new Reservation(
            reservationId,
            employeeName,
            licensePlate,
            new Date(date)
        );

        spot.AddReservation(reservation, new Date(CurrentDate()));
        await _weeklyParkingSpotRepository.UpdateAsync(spot);
    }

    public async Task UpdateAsync(ChangeReservationLicencePlate command)
    {
        var weeklyParkingSpot = await GetWeeklyParkingSpotByReservationAsync(command.ReservationId);

        if (weeklyParkingSpot is null)
        {
            throw new WeeklyParkingSpotNotFoundException();
        }

        var reservationId = new ReservationId(command.ReservationId);
        var reservation = weeklyParkingSpot.Reservations
            .SingleOrDefault(x => x.Id == reservationId);

        if (reservation is null)
        {
            throw new ReservationNotFoundException(command.ReservationId);
        }

        reservation.ChangeLicensePlate(command.LicensePlate);
        await _weeklyParkingSpotRepository.UpdateAsync(weeklyParkingSpot);
    }

    public async Task DeleteAsync(DeleteReservation command)
    {
        var weeklyParkingSpot = await GetWeeklyParkingSpotByReservationAsync(command.ReservationId);
        if (weeklyParkingSpot is null)
        {
            throw new WeeklyParkingSpotNotFoundException();
        }

        weeklyParkingSpot.DeleteReservation(command.ReservationId);
        await _weeklyParkingSpotRepository.UpdateAsync(weeklyParkingSpot);
    }

    private async Task<WeeklyParkingSpot?> GetWeeklyParkingSpotByReservationAsync(ReservationId id)
        => (await _weeklyParkingSpotRepository
                .GetAllAsync())
            .SingleOrDefault(x => x.Reservations.Any(r => r.Id == id));

    private DateTime CurrentDate() => _clockService.Current();
}