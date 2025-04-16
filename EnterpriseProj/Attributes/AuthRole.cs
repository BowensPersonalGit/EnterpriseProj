using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using EnterpriseProj.Entities;

namespace EnterpriseProj.Attributes
{
    public class AuthRole : ActionFilterAttribute
    {
        private readonly Role _requiredRole;

        public AuthRole(Role requiredRole) { _requiredRole = requiredRole; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userRole = (Role)context.HttpContext.Session.GetInt32("UserRole");

            if (userRole != _requiredRole) { context.Result = new RedirectToActionResult("Login", "Account", null); }

            base.OnActionExecuting(context);
        }
    }
}
