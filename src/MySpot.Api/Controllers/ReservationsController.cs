using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Services;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("/api/reservations")]
public sealed class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReservationDto>> Get(Guid id)
    {
        var reservation = await _reservationService.GetAsync(id);

        if (reservation is null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }

    [HttpGet]
    public async Task<ActionResult<ReservationDto[]>> Get()
    {
        return Ok(await _reservationService.GetAllWeeklyReservationsAsync());
    }

    [HttpPost]
    public async Task<ActionResult> Post(CreateReservation command)
    {
        await _reservationService.CreateAsync(command with { ReservationId = Guid.NewGuid() });
        return CreatedAtAction(nameof(Get), new { Id = command.ReservationId }, default);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Put(Guid id, ChangeReservationLicencePlate command)
    {
        await _reservationService.UpdateAsync(command with { ReservationId = id });
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _reservationService.DeleteAsync(new DeleteReservation(id));
        return NoContent();
    }
}