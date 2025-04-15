using EnterpriseProj.Entities;
using EnterpriseProj.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseProj.Controllers
{
	/* This is the API controller for the job entity.
	 * It needs have the following functionality.
	 * 
	 * GET:
	 * Getting a specific job by the ID
	 * Get a list of all jobs
	 * 
	 * POST:
	 * Create a new job.
	 *  - The job needs a title, eg "Massage Therapist"
	 * 
	 * PUT:
	 * NA
	 * 
	 *	
	 * PATCH:
	 * NA
	 * 
	 * 
	 * DELETE:
	 * NA 
	 * 
	 * 
	 */
	[Route("api/[controller]")]
	[ApiController]
	public class JobAPIController : ControllerBase
	{
		private readonly AppDbContext _context;

		public JobAPIController(AppDbContext context) { _context = context; }

		[HttpGet("{id}")]
		public async Task<IActionResult> GetJobByIdAsync(int id)
		{
			var jobDto = await _context.Jobs
				.Where(j => j.JobId == id)
				.Select(j => new JobInfo
				{
					JobId = j.JobId,
					JobName = j.JobName
				})
				.FirstOrDefaultAsync();

			if (jobDto == null)
				return NotFound();

			return Ok(jobDto);
		}

		[HttpPost("add")]
		public async Task<IActionResult> AddJobAsync([FromBody] NewJob dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var job = new Job { JobName = dto.JobName };

			_context.Jobs.Add(job);
			await _context.SaveChangesAsync();

			var createdJobDto = await _context.Jobs
				.Where(j => j.JobId == job.JobId)
				.Select(j => new JobInfo
				{
					JobId = j.JobId,
					JobName = j.JobName
				})
				.FirstAsync();

			return CreatedAtAction(nameof(GetJobByIdAsync), new { id = createdJobDto.JobId }, createdJobDto);
		}

		[HttpGet("list")]
		public async Task<IActionResult> GetAllJobsAsync()
		{
			var jobs = await _context.Jobs
				.Select(j => new JobInfo
				{
					JobId = j.JobId,
					JobName = j.JobName
				})
				.ToListAsync();

			var response = new ListJobs { Jobs = jobs };

			return Ok(response);
		}
	}
}
