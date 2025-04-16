namespace EnterpriseProj.Entities
{
    public class Claim
    {
        public int Id { get; set; }
        public ClaimStatus Status { get; set; } = ClaimStatus.NotStarted;
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
    }
}
