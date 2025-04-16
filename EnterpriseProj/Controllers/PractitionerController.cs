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

        public IActionResult Dashboard()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var now = DateTime.Now;

            var appointments = _context.Appointments
                .Include(a => a.Client)
                .Where(a => a.PractitionerId == userId && a.StartTime > now)
                .OrderBy(a => a.StartTime)
                .ToList();

            ViewBag.PractitionerId = userId;

            return View(appointments);
        }

    }
}
