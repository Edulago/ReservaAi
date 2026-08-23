namespace ReservaAi.Models
{
    public class User
    {  
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateOnly Birthdate { get; set; }
        public string PasswordHash { get; set; }
        public DateTime Created_at { get; set; }
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
