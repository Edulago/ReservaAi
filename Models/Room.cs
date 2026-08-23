namespace ReservaAi.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Created_at { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
