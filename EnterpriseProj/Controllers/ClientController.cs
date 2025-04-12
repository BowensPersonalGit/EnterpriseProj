using EnterpriseProj.Entities;
using EnterpriseProj.Messages;
using EnterpriseProj.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace EnterpriseProj.Controllers
{
    [AuthRole(Role.Client)]
    public class ClientController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public ClientController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl");
        }

        public async Task<IActionResult> Dashboard()
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");

            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/list/byClient/{userId}");
            //curl -X GET https://localhost:7202/api/AppointmentAPI/list/byClient/1 -k
            if (!response.IsSuccessStatusCode)
            {
                // fallback or error handling
                return View(new List<Appointment>());
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ListAppointmentss>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // You may need to convert AppointmentInfo → Appointment if your view is expecting full Appointment objects,
            // or ideally update your view to use `List<AppointmentInfo>` instead.
            return View(result.Appointments);
        }
    }
}
