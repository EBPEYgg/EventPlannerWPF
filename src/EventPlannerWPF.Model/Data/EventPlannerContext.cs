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
                ("Server=DESKTOP-58ATQ05\\SQLEXPRESS;Database=EventPlanner;Trusted_Connection=True;TrustServerCertificate=True");
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>()
        //        .HasMany(u => u.notes)
        //        .WithRequired(n => n.User)
        //        .HasForeignKey(n => n.UserId);

        //    base.OnModelCreating(modelBuilder);
        //}
    }
}