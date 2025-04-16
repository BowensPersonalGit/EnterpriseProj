using EnterpriseProj.Entities;
using EnterpriseProj.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseProj.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BillingAPIController : ControllerBase
	{
		private readonly AppDbContext _context;

		public BillingAPIController(AppDbContext context)
		{ _context = context; }

		//GET: For example: /api/BillingAPI/claims/status/NotStarted
		[HttpGet("claims/status/{status}")]
		public async Task<IActionResult> GetClaimsByStatusAsync(string status)
		{
			if (!Enum.TryParse<ClaimStatus>(status, ignoreCase: true, out var parsedStatus))
				return BadRequest($"Invalid status value: {status}");

			var claims = await _context.Claims
				.Where(c => c.Status == parsedStatus)
				.Select(c => new ClaimInfo
				{
					Id = c.Id,
					AppointmentId = c.AppointmentId,
					Status = c.Status.ToString()
				})
				.ToListAsync();

			return Ok(new ListClaims { Claims = claims });
		}


		//PATCH: For example: /api/BillingAPI/claims/update/5?newStatus=Completed
		[HttpPatch("claims/update/{id}")]
		public async Task<IActionResult> UpdateClaimStatusAsync(int id, [FromQuery] string newStatus)
		{
			var claim = await _context.Claims.FirstOrDefaultAsync(c => c.Id == id);
			if (claim == null)
				return NotFound();

			if (!Enum.TryParse<ClaimStatus>(newStatus, ignoreCase: true, out var parsedStatus))
				return BadRequest($"Invalid status value: {newStatus}");

			claim.Status = parsedStatus;
			await _context.SaveChangesAsync();

			return Ok(new ClaimInfo
			{
				Id = claim.Id,
				AppointmentId = claim.AppointmentId,
				Status = claim.Status.ToString()
			});
		}
	}
}
