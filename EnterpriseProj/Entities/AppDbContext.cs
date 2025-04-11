using Microsoft.EntityFrameworkCore;

namespace EnterpriseProj.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<User> Users { get; set; }
		public DbSet<Job> Jobs { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Claim)
                .WithOne(c => c.Appointment)
                .HasForeignKey<Claim>(c => c.AppointmentId);
            
            modelBuilder.Entity<Appointment>()
				.HasOne(a => a.Practitioner)
				.WithMany(u => u.PractitionerAppointments)
				.HasForeignKey(a => a.PractitionerId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Client)
                .WithMany(u => u.ClientAppointments)
                .HasForeignKey(a => a.ClientId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Job)
                .WithMany(j => j.Users)
                .HasForeignKey(u => u.JobId);

			modelBuilder.Entity<Job>().HasData(
		        new Job { JobId = 1, JobName = "Physiotherapist"},
		        new Job { JobId = 2, JobName = "Psychologist" },
		        new Job { JobId = 3, JobName = "Chiropractor" },
		        new Job { JobId = 4, JobName = "Occupational Therapist" },
		        new Job { JobId = 5, JobName = "Non-Descript Massage Persons" }
	        );

			base.OnModelCreating(modelBuilder);
        }
    }
}
