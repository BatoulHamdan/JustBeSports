using JustBeSports.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JustBeSportsAdmin.Controllers
{
    public class HomeController : Controller
    {
        #region Properties

        private readonly ILogger<HomeController> _logger;


        #endregion

        #region Constructor

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Views

        public IActionResult Index()
        {
            var isLoggedIn = HttpContext.Session.GetString("AdminUsername") != null;

            if (isLoggedIn)
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            return RedirectToAction("Login", "Admin");
        }

        #endregion

        #region Methods


        #endregion
    }

}
