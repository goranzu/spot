using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
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

    public IEnumerable<ReservationDto> GetAllWeeklyReservations()
    {
        return _weeklyParkingSpotRepository
            .GetAll()
            .SelectMany(x => x.Reservations)
            .Select(x => new ReservationDto
            {
                Id = x.Id,
                EmployeeName = x.EmployeeName,
                Date = x.Date.Value.Date
            })
            .ToList();
    }

    public ReservationDto? Get(Guid id)
    {
        return GetAllWeeklyReservations().SingleOrDefault(x => x.Id == id);
    }

    public Guid? Create(CreateReservation command)
    {
        try
        {
            var (spotId, reservationId, employeeName, licensePlate, date) = command;

            var parkingSpotId = new ParkingSpotId(spotId);
            var spot = _weeklyParkingSpotRepository.Get(parkingSpotId);
            if (spot is null)
            {
                return default;
            }

            var reservation = new Reservation(
                reservationId,
                employeeName,
                licensePlate,
                new Date(date)
            );

            spot.AddReservation(reservation, new Date(CurrentDate()));
            return reservation.Id;
        }
        catch (CustomException e)
        {
            Console.WriteLine(e);
            return default;
        }
    }

    public bool Update(ChangeReservationLicencePlate command)
    {
        try
        {
            var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);

            if (weeklyParkingSpot is null)
            {
                return false;
            }

            var reservationId = new ReservationId(command.ReservationId);
            var reservation = weeklyParkingSpot.Reservations
                .SingleOrDefault(x => x.Id == reservationId);

            if (reservation is null)
            {
                return false;
            }

            reservation.ChangeLicensePlate(command.LicensePlate);
            _weeklyParkingSpotRepository.Update(weeklyParkingSpot);
            return true;
        }
        catch (CustomException e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public bool Delete(DeleteReservation command)
    {
        try
        {
            var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);
            if (weeklyParkingSpot is null)
            {
                return false;
            }

            weeklyParkingSpot.DeleteReservation(command.ReservationId);
            _weeklyParkingSpotRepository.Update(weeklyParkingSpot);
            return true;
        }
        catch (CustomException e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    private WeeklyParkingSpot? GetWeeklyParkingSpotByReservation(ReservationId id)
        => _weeklyParkingSpotRepository
            .GetAll()
            .SingleOrDefault(x => x.Reservations.Any(r => r.Id == id));

    private DateTime CurrentDate() => _clockService.Current();
}