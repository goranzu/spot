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
    public ActionResult<ReservationDto> Get(Guid id)
    {
        var reservation = _reservationService.Get(id);

        if (reservation is null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }

    [HttpGet]
    public ActionResult<ReservationDto[]> Get()
    {
        return Ok(_reservationService.GetAllWeeklyReservations());
    }

    [HttpPost]
    public ActionResult Post(CreateReservation command)
    {
        var id = _reservationService.Create(command with { ReservationId = Guid.NewGuid() });
        if (id is null)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(Get), new { Id = id }, default);
    }

    [HttpPut("{id:guid}")]
    public ActionResult Put(Guid id, ChangeReservationLicencePlate command)
    {
        var succeeded = _reservationService.Update(command with { ReservationId = id });
        if (!succeeded)
        {
            return BadRequest();
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public ActionResult Delete(Guid id)
    {
        var deleted = _reservationService.Delete(new DeleteReservation(id));
        if (!deleted)
        {
            return BadRequest();
        }

        return NoContent();
    }
}