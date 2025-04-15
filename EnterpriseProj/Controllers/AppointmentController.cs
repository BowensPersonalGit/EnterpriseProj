using EnterpriseProj.Entities;
using EnterpriseProj.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
            _apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl"); // e.g., "https://localhost:5001/api/appointment"
            _dbContext = context;
        }

        // Book an appointment
        [HttpGet]
        public IActionResult Book(int? appointmentId)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var userId = HttpContext.Session.GetInt32("UserId");

            ViewBag.AppointmentId = appointmentId;

            if (userRole == Role.Client.ToString())
            {
                ViewBag.ClientId = userId;
                ViewBag.Clients = new SelectList(
                    _dbContext.Users.Where(u => u.Id == userId), "Id", "UserName"
                    );
                ViewBag.IsClient = true;
            }
            else
            {
                ViewBag.Clients = new SelectList(
                    _dbContext.Users.Where(u => u.Role == Role.Client), "Id", "UserName"
                );
                ViewBag.IsClient = false;
            }

            ViewBag.Appointments = new SelectList(
                _dbContext.Appointments
                    .Include(a => a.Practitioner)
                    .Where(a => !a.isBooked),
                "Id", "Title"
    );
            return View();
        }

        // Create a new appointment
        [HttpGet]
        public IActionResult Create(int practitionerId)
        {
            ViewBag.PractitionerId = practitionerId;
            return View();
        }

        // POST method for booking an appointment
        [HttpPost]
        public async Task<IActionResult> Book(int id, string title, string description)
        {
            var userId = (int)HttpContext.Session.GetInt32("UserId");

            var bookAppointmentDto = new BookAppointment
            {
                ClientId = userId,
                Title = title,
                Description = description
            };

            var content = new StringContent(JsonSerializer.Serialize(bookAppointmentDto), Encoding.UTF8, "application/json");

            // Sending a PUT request to book the appointment
            var response = await _httpClient.PutAsync($"{_apiBaseUrl}/book/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                // Redirect to the client's dashboard after booking
                return RedirectToAction("Dashboard", "Client");
            }

            // Handle error if needed (e.g., show a message if the booking fails)
            ModelState.AddModelError("", "There was an error while booking the appointment.");
            return View();
        }

        // POST method for creating a new appointment
        [HttpPost]
        public async Task<IActionResult> Create(NewAppointment model)
        {
            // if the model is invalid, return to create view
            if (!ModelState.IsValid)
            {
                ViewBag.Practitioners = new SelectList(_dbContext.Users.Where(u => u.Role == Role.Practitioner), "Id", "UserName");
                return View(model);
            }

            //var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            //// Sending a POST request to create a new appointment
            //var response = await _httpClient.PostAsync($"{_apiBaseUrl}/create", content);
            //if (response.IsSuccessStatusCode)
            //{
            //    // After creation, redirect to the practicioner's dashboard
            //    return RedirectToAction("Dashboard", "Practicioner");
            //}

            //// Handle error if needed (e.g., show a message if creation fails)
            //ModelState.AddModelError("", "There was an error while creating the appointment.");
            //return View();

            var entity = new Appointment
            {
                Title = model.Title,
                Description = model.Description,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                PractitionerId = model.PactionerId,
                ClientId = model.ClientId
            };

            _dbContext.Appointments.Add(entity);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Dashboard", "Practitioner");
        }
    }
}
