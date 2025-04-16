using EnterpriseProj.Attributes;
using EnterpriseProj.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseProj.Controllers
{
    [AuthRole(Role.Practitioner)]
    public class PractitionerController : Controller
    {
        private readonly AppDbContext _context;

        public PractitionerController(AppDbContext context) { _context = context; }

        public IActionResult Dashboard(int practitionerId)
        {
            var futureAppointments = _context.Appointments
                .Where(a => a.PractitionerId == practitionerId && a.StartTime > DateTime.Now)
                .Include(a => a.Client)
                .ToList();

            ViewBag.PractitionerId = practitionerId;
            return View(futureAppointments);
        }
    }
}
