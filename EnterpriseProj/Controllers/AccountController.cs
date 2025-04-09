using EnterpriseProj.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EnterpriseProj.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            //On login we put the users role into a session -> Grab it from the session when you need it for access reasons.
            //Feel free to add more info to the session about the user if you need to user it later on.
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetInt32("UserRole", (int)user.Role);

            // Redirect based on Role
            switch (user.Role)
            {
                case Role.Client:
                    return RedirectToAction("Dashboard", "Client");

                case Role.Practitioner:
                    return RedirectToAction("Dashboard", "Practitioner");

                case Role.Admin:
                    return RedirectToAction("Dashboard", "Admin");

                case Role.Billing:
                    return RedirectToAction("Dashboard", "Billing");

                default:
                    // Should never really reach here as role is required and only one of the three above but anyway
                    return RedirectToAction("Index", "Home");
            }
        }

        //On login page if we select a new account we go here
        [HttpGet]
        public IActionResult Register()
        {
            // Pass on the roles to the view for a drop down.
            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(Role)));
            return View();
        }

        // Form submission
        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new SelectList(Enum.GetValues(typeof(Role)));
                return View(model);
            }

            _context.Users.Add(model);
            await _context.SaveChangesAsync();
            //Send a new user back to the login page after creating an account.
            return RedirectToAction("Login");
        }
    }
}
