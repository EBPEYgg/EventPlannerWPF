using EventPlannerWPF.Model.Classes;
using Microsoft.EntityFrameworkCore;

namespace EventPlannerWPF.Model.Data
{
    public class EventPlannerContext : DbContext
    {
        public DbSet<User> User { get; set; }

        public DbSet<Note> Note { get; set; }

        public EventPlannerContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer
                ("Server=(localdb)\\MSSQLLocalDB;Database=EventPlanner;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}