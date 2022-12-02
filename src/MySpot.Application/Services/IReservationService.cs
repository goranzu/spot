﻿using MySpot.Application.Commands;
using MySpot.Application.DTO;

namespace MySpot.Application.Services;

public interface IReservationService
{
    Task<IEnumerable<ReservationDto>> GetAllWeeklyReservationsAsync();
    Task<ReservationDto?> GetAsync(Guid id);
    Task CreateAsync(CreateReservation command);
    Task UpdateAsync(ChangeReservationLicencePlate command);
    Task DeleteAsync(DeleteReservation command);
}