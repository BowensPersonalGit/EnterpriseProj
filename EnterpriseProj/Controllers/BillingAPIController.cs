using Microsoft.AspNetCore.Mvc;

namespace EnterpriseProj.Controllers
{
	public class BillingAPIController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
