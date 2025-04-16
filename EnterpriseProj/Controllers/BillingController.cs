using EnterpriseProj.Attributes;
using EnterpriseProj.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseProj.Controllers
{
    [AuthRole(Role.Billing)]
    public class BillingController : Controller
    {
        // context for database
        private AppDbContext _appDbContext;

        // controller construtor
        public BillingController(AppDbContext context)
        {
            _appDbContext = context;
        }

        // GET the dashboard for all appointments that are payed
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var upcomingAppointments = await _appDbContext.Appointments
            .Include(e => e.Practitioner)
            .Include(e => e.Claim)
            .Where(e => e.IsPaid && e.Claim != null)
            .OrderBy(e => e.Claim.Status)
            .ToListAsync();

            return View(upcomingAppointments);
        }
    }
}
