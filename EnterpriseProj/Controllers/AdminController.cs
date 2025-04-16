using EnterpriseProj.Attributes;
using EnterpriseProj.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseProj.Controllers
{
    [AuthRole(Role.Admin)]
    public class AdminController : Controller
    {
        // context for database
        private AppDbContext _appDbContext;

        // controller construtor
        public AdminController(AppDbContext context) { _appDbContext = context; }

        // GET the dashboard for all appointments in the next 7 days
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            DateTime now = DateTime.Now;
            DateTime sevenDaysAway = now.AddDays(7);

            var upcomingAppointments = await _appDbContext.Appointments
            .Where(e => e.StartTime >= now && e.StartTime <= sevenDaysAway)
            .OrderBy(e => e.StartTime)
            .ToListAsync();

            return View(upcomingAppointments);
        }
    }
}
