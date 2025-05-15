using JustBeSports.Core.Entities;
using JustBeSports.Core.Interfaces;
using JustBeSports.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JustBeSports.Controllers
{
    public class ProductController : Controller
    {
        #region Properties

        private readonly IProductService _productService;
        private readonly ICartItemService _cartItemService;
        private readonly IOrderService _orderService;

        #endregion

        #region Constructor

        public ProductController(IProductService productService, ICartItemService cartItemService, IOrderService orderService)
        {
            _productService = productService;
            _cartItemService = cartItemService;
            _orderService = orderService;
        }

        #endregion

        #region Views

        public IActionResult Index()
        {
            // This ensures a session exists
            HttpContext.Session.SetString("Initialized", "true");

            ViewBag.SessionId = HttpContext.Session.Id;

            return View();
        }

        #endregion

        #region Methods

        [HttpGet]
        public IActionResult GetAllProducts(int? categoryId)
        {
            if (categoryId.HasValue)
            {
                return Json(_productService.GetProductsByCategoryId(categoryId.Value));
            }
            else
            {
                return Json(_productService.GetListOfProducts());
            }
        }

        [HttpGet]
        public IActionResult GetProduct(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }


        #endregion
    }
}
