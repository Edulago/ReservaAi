using ReservaAi.Models;

namespace ReservaAi.ViewModels
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Status { get; set; }
        public DateTime Created_at { get; set; }

        public static RoomViewModel FromRoom(Room room) => new()
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            Description = room.Description,
            Status = room.Status,
            Created_at = room.Created_at
        };
    }
}
