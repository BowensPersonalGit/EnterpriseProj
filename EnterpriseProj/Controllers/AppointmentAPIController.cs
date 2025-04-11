using EnterpriseProj.Entities;
using EnterpriseProj.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseProj.Controllers
{
	/* This is the API controller for the appointment entity.
	 * It needs have the following functionality.
	 * 
	 * GET:
	 * Gettings all unbooked appts.
	 * Getting all the booked appts.
	 * Getting an appt by ID.
	 * Getting all appts by client
	 * Getting all appts by date
	 * Getting all appts by practitioner
	 * 
	 * POST:
	 * Create an empty appt.
	 *  - Adds a time stamp, 
	 * 
	 * PUT
	 * Book an appt. AKA, attach a client to an empty appt. 
	 *	- Adds the client ID, title, description (written by the client to help tell the practitioner what their injury is) to the appt.
	 *	
	 * PATCH
	 * 
	 * 
	 * DELETE
	 * Admin can delete an appt.
	 *  - When the client calls the office to cancel, an admin can delete the appt.
	 *  - This removes the client id, title, description, and changes it
	 */
	[Route("/api/[controller]")]
    [ApiController]
    public class AppointmentAPIController : ControllerBase
    {
		//Connect the DB to my controller
		private AppDbContext? _appDbContext;
		public AppointmentAPIController(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}

		[HttpGet("list/unbooked")]
		public async Task<IActionResult> GetUnbookedAppointmentsAsync()
		{
			var appointments = await _appDbContext.Appointments
				.Where(a => a.isBooked == false)
				.Select(a => new AppointmentInfo
				{
					Id = a.Id,
					Title = a.Title,
					Description = a.Description,
					StartTime = a.StartTime,
					EndTime = a.EndTime,
					isBooked = a.isBooked,
					IsPaid = a.IsPaid
				})
				.ToListAsync();
			return Ok(new ListAppointmentss { Appointments = appointments});
		}

		[HttpGet("list/booked")]
		public async Task<IActionResult> GetBookedAppointmentsAsync()
		{
			var appointments = await _appDbContext.Appointments
				.Where(a => a.isBooked == true)
				.Select(a => new AppointmentInfo
				{
					Id = a.Id,
					Title = a.Title,
					Description = a.Description,
					StartTime = a.StartTime,
					EndTime = a.EndTime,
					isBooked = a.isBooked,
					IsPaid = a.IsPaid
				}).ToListAsync();

			return Ok(new ListAppointmentss { Appointments = appointments });
		}

		[HttpGet("list/{id}")]
		public async Task<IActionResult> GetAppointmentByIdAsync(int id)
		{
			var a = await _appDbContext.Appointments.FindAsync(id);
			if (a == null) return NotFound();

			var dto = new AppointmentInfo
			{
				Id = a.Id,
				Title = a.Title,
				Description = a.Description,
				StartTime = a.StartTime,
				EndTime = a.EndTime,
				isBooked = a.isBooked,
				IsPaid = a.IsPaid
			};
			return Ok(dto);
		}

		[HttpGet("list/client/{clientId}")]
		public async Task<IActionResult> GetAppointmentsByClientIdAsync(int clientId)
		{
			var appointments = await _appDbContext.Appointments
				.Where(a => a.ClientId == clientId)
				.Select(a => new AppointmentInfo
				{
					Id = a.Id,
					Title = a.Title,
					Description = a.Description,
					StartTime = a.StartTime,
					EndTime = a.EndTime,
					isBooked = a.isBooked,
					IsPaid = a.IsPaid
				}).ToListAsync();

			return Ok(new ListAppointmentss { Appointments = appointments });
		}


		[HttpGet("list/date/{date}")]
		public async Task<IActionResult> GetAppointmentsByDateAsync(DateTime date)
		{
			var appointments = await _appDbContext.Appointments
				.Where(a => a.StartTime.Date == date.Date)
				.Select(a => new AppointmentInfo
				{
					Id = a.Id,
					Title = a.Title,
					Description = a.Description,
					StartTime = a.StartTime,
					EndTime = a.EndTime,
					isBooked = a.isBooked,
					IsPaid = a.IsPaid
				}).ToListAsync();

			return Ok(new ListAppointmentss { Appointments = appointments });
		}

		[HttpGet("list/practitioner/{practitionerId}")]
		public async Task<IActionResult> GetAppointmentsByPractitionerIdAsync(int practitionerId)
		{
			var appointments = await _appDbContext.Appointments
				.Where(a => a.PractitionerId == practitionerId)
				.Select(a => new AppointmentInfo
				{
					Id = a.Id,
					Title = a.Title,
					Description = a.Description,
					StartTime = a.StartTime,
					EndTime = a.EndTime,
					isBooked = a.isBooked,
					IsPaid = a.IsPaid
				}).ToListAsync();

			return Ok(new ListAppointmentss { Appointments = appointments });
		}

		[HttpPost("create")]
		public async Task<IActionResult> CreateAppointmentAsync([FromBody] NewAppointment dto)
		{
			var newAppointment = new Appointment
			{
				StartTime = dto.StartTime,
				EndTime = dto.EndTime,
				PractitionerId = dto.PactionerId,
				isBooked = false
			};

			_appDbContext.Appointments.Add(newAppointment);
			await _appDbContext.SaveChangesAsync();

			return CreatedAtAction(nameof(GetAppointmentByIdAsync), new { id = newAppointment.Id }, newAppointment);
		}

		[HttpPut("book/{id}")]
		public async Task<IActionResult> BookAppointmentAsync(int id, [FromBody] BookAppointment dto)
		{
			var existing = await _appDbContext.Appointments.FirstOrDefaultAsync(a => a.Id == id);
			if (existing == null) return NotFound();

			existing.ClientId = dto.ClientId;
			existing.Title = dto.Title;
			existing.Description = dto.Description;
			existing.isBooked = true;

			await _appDbContext.SaveChangesAsync();

			return Ok(new AppointmentInfo
			{
				Id = existing.Id,
				Title = existing.Title,
				Description = existing.Description,
				StartTime = existing.StartTime,
				EndTime = existing.EndTime,
				isBooked = existing.isBooked,
				IsPaid = existing.IsPaid
			});
		}

		[HttpDelete("delete/{id}")]
		public async Task<IActionResult> DeleteAppointmentAsync(int id)
		{
			var appointment = await _appDbContext.Appointments.FirstOrDefaultAsync(a => a.Id == id);
			if (appointment == null) return NotFound();

			_appDbContext.Appointments.Remove(appointment);
			await _appDbContext.SaveChangesAsync();

			return NoContent();
		}

	}
}
