using Microsoft.AspNetCore.Mvc;

namespace EnterpriseProj.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
