using EnterpriseProj.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.IO;

namespace EnterpriseProj.Controllers
{
    public class ClientController : Controller
    {
        private AppDbContext _appDbContext;

        public ClientController(AppDbContext appDbContext) { _appDbContext = appDbContext; }
        public IActionResult Dashboard()
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            List<Appointment> appointments = _appDbContext.Appointments.Include(a => a.Practitioner)
                .Include(a => a.Client).Where(a => a.ClientId == userId).ToList();
            return View(appointments);
        }
    }
}
