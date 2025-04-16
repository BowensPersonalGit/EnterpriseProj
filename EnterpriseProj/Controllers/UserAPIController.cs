using EnterpriseProj.Entities;
using EnterpriseProj.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseProj.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserAPIController : ControllerBase
	{
		private readonly AppDbContext _context;

		public UserAPIController(AppDbContext context) { _context = context; }

		// GET: /api/UserAPI/{id}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetUserByIdAsync(int id)
		{
			var user = await _context.Users
				.Include(u => u.Job)
				.FirstOrDefaultAsync(u => u.Id == id);

			if (user == null)
				return NotFound();

			var userInfo = new UserInfo
			{
				Id = user.Id,
				Name = user.Name,
				Role = user.Role.ToString(),
				JobName = user.Job?.JobName
			};

			return Ok(userInfo);
		}
	}
}
