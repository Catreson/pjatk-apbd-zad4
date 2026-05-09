using Zadanie4.Models;

namespace Zadanie4.Data;
public static class AppData
{
    public static List<Room> Rooms { get; } = new()
    {
        new Room { Id = 1, Name = "Sala 101", BuildingCode = "A", Floor = 1, Capacity = 21, HasProjector = true,  IsActive = true },
        new Room { Id = 2, Name = "Sala 213", BuildingCode = "A", Floor = 2, Capacity = 37, HasProjector = false, IsActive = true },
        new Room { Id = 3, Name = "Sala 301",  BuildingCode = "B", Floor = 3, Capacity = 69, HasProjector = true,  IsActive = false },
        new Room { Id = 4, Name = "Sala 1", BuildingCode = "B", Floor = 1, Capacity = 420, HasProjector = true,  IsActive = true},
        new Room { Id = 5, Name = "Sala 210", BuildingCode = "H", Floor = 2, Capacity = 96, HasProjector = false, IsActive = true },
    };

    public static List<Reservation> Reservations { get; } = new()
    {
        new Reservation { Id = 1, RoomId = 1, OrganizerName = "Jan Paweł",  Topic = "APBD",    Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(8,  0), EndTime = new TimeOnly(10, 0), Status = "confirmed" },
        new Reservation { Id = 2, RoomId = 1, OrganizerName = "Robert Kubica",    Topic = "TPO",    Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(13, 0), Status = "planned" },
        new Reservation { Id = 3, RoomId = 2, OrganizerName = "Andrzej Kajak",  Topic = "CHWDP",   Date = new DateOnly(2026, 5, 11), StartTime = new TimeOnly(9,  0), EndTime = new TimeOnly(11, 0), Status = "confirmed" },
        new Reservation { Id = 4, RoomId = 3, OrganizerName = "Janusz Kowalski", Topic = "RBD",       Date = new DateOnly(2026, 5, 12), StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(16, 0), Status = "planned" },
        new Reservation { Id = 5, RoomId = 5, OrganizerName = "Anna Nowak", Topic = "MAS",  Date = new DateOnly(2026, 5, 13), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(12, 0), Status = "cancelled" },
    };

    public static int NextRoomId() => Rooms.Count == 0 ? 1 : Rooms.Max(r => r.Id) + 1;
    public static int NextReservationId() => Reservations.Count == 0 ? 1 : Reservations.Max(r => r.Id) + 1;
}
