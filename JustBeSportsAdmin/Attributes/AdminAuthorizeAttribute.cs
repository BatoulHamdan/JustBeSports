using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AdminAuthorizeAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        var username = httpContext.Session.GetString("AdminUsername");
        var role = httpContext.Session.GetString("AdminRole");

        if (string.IsNullOrEmpty(username))
        {
            context.Result = new RedirectToActionResult("Login", "Admin", null);
            return;
        }

        // Access controller and set ViewBag values
        if (context.Controller is Controller controller)
        {
            controller.ViewBag.IsAdminLoggedIn = true;
            controller.ViewBag.IsMainAdmin = role == "main";
        }

        base.OnActionExecuting(context);
    }
}
