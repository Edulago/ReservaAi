namespace ReservaAi.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public int Status { get; set; }
        public DateTime Created_at { get; set; }
    }
}
