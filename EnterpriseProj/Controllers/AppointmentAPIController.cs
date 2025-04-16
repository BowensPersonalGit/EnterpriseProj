using EnterpriseProj.Entities;
using EnterpriseProj.Messages;
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
	 * Updating an appointment to mark it as paid
	 * 
	 * DELETE
	 * Admin can cancel an appt.
	 *  - When the client calls the office to cancel, an admin can cancel the appt.
	 *  - This removes the client id, title, description, and changes it
	 */
	[Route("/api/[controller]")]
    [ApiController]
    public class AppointmentAPIController : ControllerBase
    {
		//Connect the DB to my controller
		private AppDbContext? _appDbContext;
		public AppointmentAPIController(AppDbContext appDbContext) { _appDbContext = appDbContext; }

		[HttpGet("list/ByUnbooked")]
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
					IsPaid = a.IsPaid,
					ClientId = a.ClientId,
					ClientName = a.Client != null ? a.Client.Name : null,
					PractitionerId = a.PractitionerId,
					PractitionerName = a.Practitioner != null ? a.Practitioner.Name : null,
					PractitionerJob = a.Practitioner != null && a.Practitioner.Job != null ? a.Practitioner.Job.JobName : null
				})
				.ToListAsync();
			return Ok(new ListAppointments { Appointments = appointments});
		}

		[HttpGet("list/byBooked")]
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
					IsPaid = a.IsPaid,
					ClientId = a.ClientId,
					ClientName = a.Client != null ? a.Client.Name : null,
					PractitionerId = a.PractitionerId,
					PractitionerName = a.Practitioner != null ? a.Practitioner.Name : null,
					PractitionerJob = a.Practitioner != null && a.Practitioner.Job != null ? a.Practitioner.Job.JobName : null

				}).ToListAsync();

			return Ok(new ListAppointments { Appointments = appointments });
		}

		[HttpGet("list/byId/{id}")]
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
				IsPaid = a.IsPaid,
				ClientId = a.ClientId,
				ClientName = a.Client != null ? a.Client.Name : null,
				PractitionerId = a.PractitionerId,
				PractitionerName = a.Practitioner != null ? a.Practitioner.Name : null,
				PractitionerJob = a.Practitioner != null && a.Practitioner.Job != null ? a.Practitioner.Job.JobName : null

			};
			return Ok(dto);
		}

		[HttpGet("list/byClient/{clientId}")]
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
					IsPaid = a.IsPaid,
					ClientId = a.ClientId,
					ClientName = a.Client != null ? a.Client.Name : null,
					PractitionerId = a.PractitionerId,
					PractitionerName = a.Practitioner != null ? a.Practitioner.Name : null,
					PractitionerJob = a.Practitioner != null && a.Practitioner.Job != null ? a.Practitioner.Job.JobName : null

				}).ToListAsync();

			return Ok(new ListAppointments { Appointments = appointments });
		}


		[HttpGet("list/byDate/{date}")]
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
					IsPaid = a.IsPaid,
					ClientId = a.ClientId,
					ClientName = a.Client != null ? a.Client.Name : null,
					PractitionerId = a.PractitionerId,
					PractitionerName = a.Practitioner != null ? a.Practitioner.Name : null,
					PractitionerJob = a.Practitioner != null && a.Practitioner.Job != null ? a.Practitioner.Job.JobName : null

				}).ToListAsync();

			return Ok(new ListAppointments { Appointments = appointments });
		}

		[HttpGet("list/byPractitioner/{practitionerId}")]
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
					IsPaid = a.IsPaid,
					ClientId = a.ClientId,
					ClientName = a.Client != null ? a.Client.Name : null,
					PractitionerId = a.PractitionerId,
					PractitionerName = a.Practitioner != null ? a.Practitioner.Name : null,
					PractitionerJob = a.Practitioner != null && a.Practitioner.Job != null ? a.Practitioner.Job.JobName : null

				}).ToListAsync();

			return Ok(new ListAppointments { Appointments = appointments });
		}

		[HttpPost("create")]
		public async Task<IActionResult> CreateAppointmentAsync([FromBody] NewAppointment dto)
		{
			var newAppointment = new Appointment
			{
				StartTime = dto.StartTime,
				EndTime = dto.EndTime,
				PractitionerId = dto.PractitionerId,
                Title = dto.Title,
                Description = dto.Description,
                isBooked = false,
				ClientId = dto.ClientId
			};

			_appDbContext.Appointments.Add(newAppointment);
			await _appDbContext.SaveChangesAsync();

			return CreatedAtAction(nameof(GetAppointmentByIdAsync), new { id = newAppointment.Id }, newAppointment);
		}

		[HttpPut("book/{id}")]
		public async Task<IActionResult> BookAppointmentAsync(int id, [FromBody] BookAppointment dto)
		{
			var a = await _appDbContext.Appointments.FirstOrDefaultAsync(a => a.Id == id);
			if (a == null) return NotFound();

			a.ClientId = dto.ClientId;
			a.Title = dto.Title;
			a.Description = dto.Description;
			a.isBooked = true;

			await _appDbContext.SaveChangesAsync();

			return Ok(new AppointmentInfo
			{
				Id = a.Id,
				Title = a.Title,
				Description = a.Description,
				StartTime = a.StartTime,
				EndTime = a.EndTime,
				isBooked = a.isBooked,
				IsPaid = a.IsPaid,
				ClientId = a.ClientId,
				ClientName = a.Client != null ? a.Client.Name : null,
				PractitionerId = a.PractitionerId,
				PractitionerName = a.Practitioner != null ? a.Practitioner.Name : null,
				PractitionerJob = a.Practitioner != null && a.Practitioner.Job != null ? a.Practitioner.Job.JobName : null
			});
		}

		[HttpPatch("pay/{id}")]
		public async Task<IActionResult> PayForAppointmentAsync(int id)
		{
			var appointment = await _appDbContext.Appointments
				.Include(a => a.Client)
				.Include(a => a.Practitioner)
					.ThenInclude(p => p.Job)
				.FirstOrDefaultAsync(a => a.Id == id);

			if (appointment == null)
				return NotFound();

			appointment.IsPaid = true;
			await _appDbContext.SaveChangesAsync();

			var result = new AppointmentInfo
			{
				Id = appointment.Id,
				Title = appointment.Title,
				Description = appointment.Description,
				StartTime = appointment.StartTime,
				EndTime = appointment.EndTime,
				isBooked = appointment.isBooked,
				IsPaid = appointment.IsPaid,
				ClientId = appointment.ClientId,
				ClientName = appointment.Client?.Name,
				PractitionerId = appointment.PractitionerId,
				PractitionerName = appointment.Practitioner?.Name,
				PractitionerJob = appointment.Practitioner?.Job?.JobName
			};

			return Ok(result);
		}


		[HttpDelete("cancel/{id}")]
		public async Task<IActionResult> DeleteAppointmentAsync(int id)
		{
			var appointment = await _appDbContext.Appointments
				.Include(a => a.Client)
				.Include(a => a.Practitioner)
					.ThenInclude(p => p.Job)
				.FirstOrDefaultAsync(a => a.Id == id);

			if (appointment == null)
				return NotFound();

			// Clear client-specific info
			appointment.ClientId = null;
			appointment.Title = null;
			appointment.Description = null;
			appointment.isBooked = false;

			await _appDbContext.SaveChangesAsync();

			// Return updated DTO
			var result = new AppointmentInfo
			{
				Id = appointment.Id,
				Title = appointment.Title,
				Description = appointment.Description,
				StartTime = appointment.StartTime,
				EndTime = appointment.EndTime,
				isBooked = appointment.isBooked,
				IsPaid = appointment.IsPaid,
				ClientId = appointment.ClientId,
				ClientName = appointment.Client?.Name,
				PractitionerId = appointment.PractitionerId,
				PractitionerName = appointment.Practitioner?.Name,
				PractitionerJob = appointment.Practitioner?.Job?.JobName
			};

			return Ok(result);
		}


        [HttpGet("practitioners")]
        public async Task<IActionResult> GetAllPractitioners()
        {
            var users = await _appDbContext.Users
                .Where(u => u.Role == Role.Practitioner)
                .Select(u => new UserInfo
                {
                    UserId = u.Id,
                    UserName = u.Name
                }).ToListAsync();

            return Ok(new ListUsers { Users = users });
        }

        [HttpGet("clients")]
        public async Task<IActionResult> GetAllClients()
		{
            var users = await _appDbContext.Users
				.Where (u => u.Role == Role.Client)
                .Select(u => new UserInfo
                {
					UserId = u.Id,
					UserName = u.Name
                }).ToListAsync();

            return Ok(new ListUsers { Users = users});
        }
    }
}
