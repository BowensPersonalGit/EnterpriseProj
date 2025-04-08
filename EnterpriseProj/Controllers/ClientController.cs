using Microsoft.AspNetCore.Mvc;

namespace EnterpriseProj.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
