namespace EnterpriseProj.Messages
{
	/* This class is for the response message when a practitioner is trying to create a new appointment.
	 * The only thing that is needed is the start time, end time, and the practitioner ID.
	 */
	public class NewAppointment
	{
		public int AppointmentId { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public int PactionerId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
