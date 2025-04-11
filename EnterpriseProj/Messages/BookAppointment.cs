using System.ComponentModel.DataAnnotations;

namespace EnterpriseProj.Messages
{
	/* * This class is for the response message when a client is trying to book an appointment.
	 * The only thing that is needed is the appointment ID, client ID, title, and description.
	 */
	public class BookAppointment
	{
		public int AppointmentId { get; set; }
		public int ClientId { get; set; }
		[Required(ErrorMessage = "Please add a title for the appointment.")]
		public string? Title { get; set; }
		[Required(ErrorMessage = "Please add a description outlining what your ailment is to allow the practioner to be prepare to give you the best treatment possible.")]
		public string? Description { get; set; }

	}
}
