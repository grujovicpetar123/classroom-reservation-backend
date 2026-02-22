using ClassroomReservationBackend.Model.DTO.ReservationDTO;
using ClassroomReservationBackend.Service.ReservationService;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassroomReservationBackend.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] ReservationFilterRequest filter)
    {
        var reservations = await _reservationService.GetAllAsync(filter);
        return Ok(reservations);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMy([FromQuery] ReservationFilterRequest filter)
    {
        var userId = GetUserId();
        var reservations = await _reservationService.GetMyReservationsAsync(userId, filter);
        return Ok(reservations);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var reservation = await _reservationService.GetByIdAsync(id);

        if (!IsAdmin() && reservation.UserId != GetUserId())
            return Forbid();

        return Ok(reservation);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservationRequest request)
    {
        var userId = GetUserId();
        var reservation = await _reservationService.CreateAsync(userId, request);
        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReservationRequest request)
    {
        var reservation = await _reservationService.UpdateAsync(id, GetUserId(), IsAdmin(), request);
        return Ok(reservation);
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] ReservationStatusRequest request)
    {
        var reservation = await _reservationService.UpdateStatusAsync(id, GetUserId(), request);
        return Ok(reservation);
    }

    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var userId = GetUserId();
        var existing = await _reservationService.GetByIdAsync(id);

        if (existing.UserId != userId && !IsAdmin())
            return Forbid();

        var result = await _reservationService.UpdateStatusAsync(id, userId,
            new ReservationStatusRequest { Status = "Cancelled" });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _reservationService.DeleteAsync(id, GetUserId(), IsAdmin());
        return NoContent();
    }

    private Guid GetUserId()
    {
        var value = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? User.FindFirst("sub")?.Value
                    ?? throw new UnauthorizedAccessException("User ID not found in token.");
        return Guid.Parse(value);
    }

    private bool IsAdmin() => User.IsInRole("Admin");
}