using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AdminBaseController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var username = HttpContext.Session.GetString("AdminUsername");
        var role = HttpContext.Session.GetString("AdminRole");

        var actionName = context.ActionDescriptor.RouteValues["action"] ?? "";
        if (actionName != "Login" && actionName != "Logout" && string.IsNullOrEmpty(username))
        {
            context.Result = new RedirectToActionResult("Login", "Admin", null);
            return;
        }

        ViewBag.IsAdminLoggedIn = !string.IsNullOrEmpty(username);
        ViewBag.IsMainAdmin = role == "main";

        base.OnActionExecuting(context);
    }

    protected bool IsMainAdmin()
    {
        return HttpContext.Session.GetString("AdminRole") == "main";
    }
}
