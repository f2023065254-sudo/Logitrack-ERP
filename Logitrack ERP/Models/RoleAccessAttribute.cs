using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Logitrack_ERP.Filters
{
    public class RoleAccessAttribute : ActionFilterAttribute
    {
        private readonly string[] _allowedRoles;

        public RoleAccessAttribute(params string[] roles)
        {
            _allowedRoles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Aapke AccountController wale session se role read kar raha hai
            var userRole = context.HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userRole))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            if (!_allowedRoles.Contains(userRole))
            {
                context.Result = new ContentResult
                {
                    Content = $"ACCESS DENIED: Aapka role '{userRole}' hai. Aapko is page ki permission nahi hai.",
                    StatusCode = 403,
                    ContentType = "text/plain"
                };
            }
            base.OnActionExecuting(context);
        }
    }
}