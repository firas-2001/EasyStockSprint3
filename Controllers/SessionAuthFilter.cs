using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EasyStock.Controllers
{
    public class SessionAuthFilter : IActionFilter
    {
        private readonly string[] _requiredRoles;

        public SessionAuthFilter(string requiredRoles = "")
        {
            _requiredRoles = string.IsNullOrWhiteSpace(requiredRoles)
                ? Array.Empty<string>()
                : requiredRoles.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var isAuthenticated = session.GetString("IsAuthenticated") == "true";

            if (!isAuthenticated)
            {
                var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                context.Result = new RedirectToActionResult("Login", "Auth", new { returnUrl });
                return;
            }

            if (_requiredRoles.Length == 0)
            {
                return;
            }

            var userRole = session.GetString("Role") ?? string.Empty;
            if (!_requiredRoles.Contains(userRole, StringComparer.OrdinalIgnoreCase))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
