using ReservaAi.Models;

namespace ReservaAi.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateOnly Birthdate { get; set; }
        public DateTime Created_at { get; set; }

        public static UserViewModel FromUser(User user) => new()
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            Birthdate = user.Birthdate,
            Created_at = user.Created_at
        };
    }
}
