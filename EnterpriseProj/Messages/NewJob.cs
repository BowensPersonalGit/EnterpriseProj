using System.ComponentModel.DataAnnotations;

namespace EnterpriseProj.Messages
{
	public class NewJob
	{
		[Required(ErrorMessage = "Job name is required.")]
		public string JobName { get; set; }
	}
}
