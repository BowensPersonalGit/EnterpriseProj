using System.ComponentModel.DataAnnotations;

namespace EnterpriseProj.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Appointment Title is Required")]
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int? ClaimId { get; set; }
        public Claim? Claim { get; set; }
        public int? PractitionerId { get; set; }
        public User? Practitioner { get; set; }
        public int? ClientId { get; set; }
        public User? Client { get; set; }
        public bool isBooked { get; set; } = false;
    }
}
