using EnterpriseProj.Entities;
using EnterpriseProj.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace EnterpriseProj.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly AppDbContext _dbContext;

        public AppointmentController(HttpClient httpClient, IConfiguration configuration, AppDbContext context)
        {
            _httpClient = httpClient;
            _dbContext = context;
            _apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl");
        }

        // Book an appointment
        [HttpGet]
        public IActionResult BookClient(int? appointmentId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var model = new BookAppointment
            {
                AppointmentId = appointmentId ?? 0,
                ClientId = userId ?? 0
            };

            ViewBag.IsClient = true;

            var appointments = _dbContext.Appointments
                .Where(a => !a.isBooked)
                .ToList();

            ViewBag.Appointments = new SelectList(appointments, "Id", "Title");

            return View(model);
        }

        [HttpGet]
        public IActionResult BookAdmin()
        {
            var model = new BookAppointment();

            ViewBag.IsClient = false;

            // Load unbooked appointments for admins
            var appointments = _dbContext.Appointments
                .Where(a => !a.isBooked)  // Ensure you are selecting only unbooked appointments
                .ToList();

            if (appointments.Any())
            {
                ViewBag.Appointments = new SelectList(appointments, "Id", "Title");
            }
            else
            {
                ViewBag.Appointments = new SelectList(new List<string>());
            }

            var clients = _dbContext.Users
                .Where(u => u.Role == Role.Client)
                .ToList();

            if (clients.Any())
            {
                ViewBag.Clients = new SelectList(clients, "Id", "UserName");
            }
            else
            {
                ViewBag.Clients = new SelectList(new List<string>());
            }

            return View(model);
        }


        // Create a new appointment
        [HttpGet]
        public IActionResult Create(int practitionerId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            ViewBag.PractitionerId = userId;
            return View();
        }

        // POST method for booking an appointment
        [HttpPost]
        public async Task<IActionResult> Book(BookAppointment model)
        {
            // if invalid will need to inject the dropdowns again
            if (!ModelState.IsValid)
            {
                ViewBag.Clients = new SelectList(_dbContext.Users.Where(u => u.Role == Role.Client), "Id", "UserName");
                ViewBag.Appointments = new SelectList(_dbContext.Appointments.Where(a => !a.isBooked), "Id", "Title");
                return View(model);
            }

            // make sure client exists
            var clientExists = await _dbContext.Users
                .AnyAsync(u => u.Id == model.ClientId && u.Role == Role.Client);

            if (!clientExists)
            {
                ModelState.AddModelError(nameof(model.ClientId), "Selected client does not exist.");
                ViewBag.Clients = new SelectList(_dbContext.Users.Where(u => u.Role == Role.Client), "Id", "UserName");
                ViewBag.Appointments = new SelectList(_dbContext.Appointments.Where(a => !a.isBooked), "Id", "Title");
                return View(model);
            }

            var entity = await _dbContext.Appointments.FindAsync(model.AppointmentId);
            if (entity == null)
            {
                ModelState.AddModelError("", "Appointment not found.");
                ViewBag.Clients = new SelectList(_dbContext.Users.Where(u => u.Role == Role.Client), "Id", "UserName");
                ViewBag.Appointments = new SelectList(_dbContext.Appointments.Where(a => !a.isBooked), "Id", "Title");
                return View(model);
            }

            entity.Title = model.Title;
            entity.Description = model.Description;
            entity.ClientId = model.ClientId;
            entity.isBooked = true;

            _dbContext.Appointments.Update(entity);
            await _dbContext.SaveChangesAsync();

            var userId = HttpContext.Session.GetInt32("UserId");
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                switch (user.Role)
                {
                    case Role.Admin:
                        return RedirectToAction("Dashboard", "Admin");
                    case Role.Client:
                        return RedirectToAction("Dashboard", "Client");
                }
            }

            // in case everything else fails
            return RedirectToAction("Index", "Home");
        }


        // POST method for creating a new appointment
        [HttpPost]
        public async Task<IActionResult> Create(NewAppointment model)
        {
            // if the model is invalid, return to create view
            if (!ModelState.IsValid)
            {
                ViewBag.Practitioners = HttpContext.Session.GetInt32("UserId");
                return View(model);
            }

            var entity = new Appointment
            {
                Title = model.Title,
                Description = model.Description,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                PractitionerId = model.PractitionerId,
            };

            _dbContext.Appointments.Add(entity);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Dashboard", "Practitioner");
        }
    }
}
