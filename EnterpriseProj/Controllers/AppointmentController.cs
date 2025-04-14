using EnterpriseProj.Messages;
using Microsoft.AspNetCore.Mvc;
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

        public AppointmentController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl"); // e.g., "https://localhost:5001/api/appointment"
        }

        // Book an appointment
        [HttpGet]
        public IActionResult Book(int id)
        {
            // This page will display the details for booking an appointment
            ViewBag.AppointmentId = id;
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
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            // Sending a POST request to create a new appointment
            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/create", content);
            if (response.IsSuccessStatusCode)
            {
                // After creation, redirect to the client's dashboard
                return RedirectToAction("Dashboard", "Client");
            }

            // Handle error if needed (e.g., show a message if creation fails)
            ModelState.AddModelError("", "There was an error while creating the appointment.");
            return View();
        }
    }
}
