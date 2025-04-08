using Microsoft.EntityFrameworkCore;

namespace EnterpriseProj.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Claim)
                .WithOne(c => c.Appointment)
                .HasForeignKey<Claim>(c => c.AppointmentId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
