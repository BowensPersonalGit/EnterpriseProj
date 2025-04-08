using EnterpriseProj.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseProj.Controllers
{
    public class PractitionerController : Controller
    {
        private readonly AppDbContext _context;

        public PractitionerController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
