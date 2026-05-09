using Microsoft.AspNetCore.Mvc;
using Zadanie4.Data;
using Zadanie4.Models;

namespace Zadanie4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] DateOnly? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId)
    {
        var reservations = AppData.Reservations.AsEnumerable();

        if (date.HasValue)
            reservations = reservations.Where(r => r.Date == date.Value);

        if (!string.IsNullOrWhiteSpace(status))
            reservations = reservations.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));

        if (roomId.HasValue)
            reservations = reservations.Where(r => r.RoomId == roomId.Value);

        return Ok(reservations.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var reservation = AppData.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
            return NotFound($"Reservation with id {id} not found.");

        return Ok(reservation);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Reservation reservation)
    {
        var room = AppData.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
        if (room is null)
            return NotFound($"Room with id {reservation.RoomId} not found.");

        if (!room.IsActive)
            return BadRequest($"Room {reservation.RoomId} is not active.");

        bool hasConflict = AppData.Reservations.Any(r =>
            r.RoomId == reservation.RoomId &&
            r.Date == reservation.Date &&
            r.StartTime < reservation.EndTime &&
            reservation.StartTime < r.EndTime);

        if (hasConflict)
            return Conflict("Reservation overlaps with an existing reservation for this room.");

        reservation.Id = AppData.NextReservationId();
        AppData.Reservations.Add(reservation);

        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Reservation updated)
    {
        var reservation = AppData.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
            return NotFound($"Reservation with id {id} not found.");

        bool hasConflict = AppData.Reservations.Any(r =>
            r.Id != id &&
            r.RoomId == updated.RoomId &&
            r.Date == updated.Date &&
            r.StartTime < updated.EndTime &&
            updated.StartTime < r.EndTime);

        if (hasConflict)
            return Conflict("Updated reservation overlaps with an existing reservation for this room.");

        reservation.RoomId        = updated.RoomId;
        reservation.OrganizerName = updated.OrganizerName;
        reservation.Topic         = updated.Topic;
        reservation.Date          = updated.Date;
        reservation.StartTime     = updated.StartTime;
        reservation.EndTime       = updated.EndTime;
        reservation.Status        = updated.Status;

        return Ok(reservation);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var reservation = AppData.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
            return NotFound($"Reservation with id {id} not found.");

        AppData.Reservations.Remove(reservation);
        return NoContent();
    }
}
