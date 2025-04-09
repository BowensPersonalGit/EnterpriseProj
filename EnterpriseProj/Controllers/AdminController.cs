using EnterpriseProj.Attributes;
using EnterpriseProj.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseProj.Controllers
{
    [AuthRole(Role.Admin)]
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
