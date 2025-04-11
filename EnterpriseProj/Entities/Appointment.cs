using System.ComponentModel.DataAnnotations;

namespace EnterpriseProj.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        //Attributes
        public string? Title { get; set; } = null;
		public string? Description { get; set; } = null;

		[Required(ErrorMessage = "Please add a start time.")]
		public DateTime StartTime { get; set; }
		[Required(ErrorMessage = "Please add an end time.")]
		public DateTime EndTime { get; set; }

		public bool isBooked { get; set; } = false;
        public bool IsPaid { get; set; } = false;

		//References to other entities in the database
		//One to zero-one relationship with claim
		public int? ClaimId { get; set; }
        public Claim? Claim { get; set; }

		//One to many relationship with practitioner
		public int? PractitionerId { get; set; }
        public User? Practitioner { get; set; }

		//One to many relationship with client
		public int? ClientId { get; set; }
        public User? Client { get; set; }
    }
}
