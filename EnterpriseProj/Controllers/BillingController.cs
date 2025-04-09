using EnterpriseProj.Attributes;
using EnterpriseProj.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseProj.Controllers
{
    [AuthRole(Role.Billing)]
    public class BillingController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
