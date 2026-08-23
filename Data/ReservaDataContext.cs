using ReservaAi.Models;
using Microsoft.EntityFrameworkCore;

namespace ReservaAi.Data
{
    public class ReservaDataContext : DbContext
    {
        public ReservaDataContext(DbContextOptions<ReservaDataContext> options) : base(options)
        {
        }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }

         
    }
}
