using JustBeSports.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JustBeSportsAdmin.Controllers
{
    public class AdminController : Controller
    {
        #region Properties

        private readonly IAdminService _adminService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        #endregion

        #region Constructor

        public AdminController(
            IAdminService adminService,
            ICategoryService categoryService,
            IProductService productService,
            IOrderService orderService)
        {
            _adminService = adminService;
            _categoryService = categoryService;
            _productService = productService;
            _orderService = orderService;
        }

        #endregion

        #region Action Filter

        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    string actionName = context.ActionDescriptor.RouteValues["action"] ?? "";

        //    // Allow only Login and Logout actions without authentication
        //    if (actionName != "Login" && actionName != "Logout")
        //    {
        //        if (!IsAdminLoggedIn())
        //        {
        //            context.Result = new RedirectToActionResult("Login", "Admin", null);
        //            return;
        //        }
        //    }

        //    // Set ViewBag values for use in views
        //    ViewBag.IsAdminLoggedIn = IsAdminLoggedIn();
        //    ViewBag.IsMainAdmin = IsMainAdmin();

        //    base.OnActionExecuting(context);
        //}

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string actionName = context.ActionDescriptor.RouteValues["action"] ?? "";

            if (actionName == "Create")
            {
                var adminsExist = _adminService.GetAllAdmins().Any();
                if (!adminsExist)
                {
                    // No admins exist yet, allow Create page to be accessed freely
                    base.OnActionExecuting(context);
                    return;
                }
            }

            if (actionName != "Login" && actionName != "Logout")
            {
                if (!IsAdminLoggedIn())
                {
                    context.Result = new RedirectToActionResult("Login", "Admin", null);
                    return;
                }
            }

            ViewBag.IsAdminLoggedIn = IsAdminLoggedIn();
            ViewBag.IsMainAdmin = IsMainAdmin();

            base.OnActionExecuting(context);
        }


        #endregion

        #region Helper Functions

        private bool IsAdminLoggedIn()
        {
            return HttpContext.Session.GetString("AdminUsername") != null;
        }

        private bool IsMainAdmin()
        {
            return HttpContext.Session.GetString("AdminRole") == "main";
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        #endregion

        #region Views

        public IActionResult Index()
        {
            var admins = _adminService.GetAllAdmins();
            return View(admins);
        }

        public IActionResult Login() => View();

        public IActionResult Create() {
            //if (!IsMainAdmin())
            //    return RedirectToAction("Index", "Home");
            return View();
        }

        public IActionResult Edit(int id)
        {
            if (!IsMainAdmin())
                return RedirectToAction("Index", "Home");

            var admin = _adminService.GetAdminById(id);
            return View(admin);
        }

        public IActionResult Dashboard()
        {
            ViewBag.CategoryCount = _categoryService.GetListOfCategories().Count;
            ViewBag.ProductCount = _productService.GetListOfProducts().Count;
            ViewBag.OrderCount = _orderService.GetListOfOrders().Count;
            return View();
        }

        #endregion

        #region Methods

        [HttpPost]
        public IActionResult Create(string Username, string Password, string Role)
        {
            var existingAdmin = _adminService.GetAllAdmins()
                .FirstOrDefault(a => a.Username == Username);

            if (existingAdmin != null)
            {
                ViewBag.Error = "Username already exists.";
                return View(); 
            }

            var passwordHash = HashPassword(Password);
            _adminService.AddAdmin(Username, passwordHash, Role);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(int Id, string Username, string Password, string Role)
        {
            if (!IsMainAdmin())
                return RedirectToAction("Index", "Home");

            var passwordHash = string.IsNullOrEmpty(Password)
                ? _adminService.GetAdminById(Id).PasswordHash
                : HashPassword(Password);

            _adminService.UpdateAdmin(Id, Username, passwordHash, Role);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            //if (!IsMainAdmin())
            //    return RedirectToAction("Index", "Home");

            _adminService.DeleteAdmin(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {
            var admin = _adminService.GetAllAdmins()
                .FirstOrDefault(a => a.Username == Username);

            if (admin != null && BCrypt.Net.BCrypt.Verify(Password, admin.PasswordHash))
            {
                HttpContext.Session.SetInt32("AdminId", admin.Id);
                HttpContext.Session.SetString("AdminUsername", admin.Username);
                HttpContext.Session.SetString("AdminRole", admin.Role);

                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        #endregion
    }
}
