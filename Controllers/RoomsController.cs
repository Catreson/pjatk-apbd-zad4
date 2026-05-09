using Microsoft.AspNetCore.Mvc;
using Zadanie4.Data;
using Zadanie4.Models;

namespace Zadanie4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        var rooms = AppData.Rooms.AsEnumerable();

        if (minCapacity.HasValue)
            rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);

        if (hasProjector.HasValue)
            rooms = rooms.Where(r => r.HasProjector == hasProjector.Value);

        if (activeOnly.HasValue)
            rooms = rooms.Where(r => r.IsActive == activeOnly.Value);
        return Ok(rooms.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var room = AppData.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
            return NotFound($"Room with id {id} not found.");

        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public IActionResult GetByBuilding(string buildingCode)
    {
        var rooms = AppData.Rooms
            .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(rooms);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Room room)
    {
        room.Id = AppData.NextRoomId();
        AppData.Rooms.Add(room);

        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Room updated)
    {
        var room = AppData.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
            return NotFound($"Room with id {id} not found.");

        room.Name         = updated.Name;
        room.BuildingCode = updated.BuildingCode;
        room.Floor        = updated.Floor;
        room.Capacity     = updated.Capacity;
        room.HasProjector = updated.HasProjector;
        room.IsActive     = updated.IsActive;

        return Ok(room);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var room = AppData.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
            return NotFound($"Room with id {id} not found.");

        bool hasReservations = AppData.Reservations.Any(r => r.RoomId == id);
        if (hasReservations)
            return Conflict($"Room {id} has existing reservations and cannot be deleted.");

        AppData.Rooms.Remove(room);
        return NoContent();
    }
}
