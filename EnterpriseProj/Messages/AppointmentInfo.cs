using EnterpriseProj.Entities;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseProj.Messages
{
	public class AppointmentInfo
	{
		public int Id { get; set; }

		public string? Title { get; set; } = null;
		public string? Description { get; set; } = null;
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }

		public bool isBooked { get; set; } = false;
		public bool IsPaid { get; set; } = false;
	}
}
