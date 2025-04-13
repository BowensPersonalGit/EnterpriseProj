namespace EnterpriseProj.Messages
{
	public class UserInfo
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Role { get; set; } = string.Empty;
		public string? JobName { get; set; }
		public int UserId { get; set; }
		public string? UserName { get; set; }
    }
}
