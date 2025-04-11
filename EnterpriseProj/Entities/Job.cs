using System.ComponentModel.DataAnnotations;

namespace EnterpriseProj.Entities
{
	public class Job
	{
		public int JobId { get; set; }

		[Required(ErrorMessage = "Please enter the name of the job.")]
		public string? JobName { get; set; }

		//One-to-many relationship with practioners
		//A job can be assigned to many users
		public ICollection<User>? Users { get; set; } = new List<User>();
	}
}
