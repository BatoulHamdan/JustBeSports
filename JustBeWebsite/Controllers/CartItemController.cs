using JustBeSports.Core.Entities;
using JustBeSports.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JustBeSports.Controllers
{
    public class CartItemController : Controller
    {
        #region Properties

        private readonly IProductService _productService;
        private readonly IProductVariantService _productVariantService;
        private readonly ICartItemService _cartItemService;
        private readonly IOrderService _orderService;

        #endregion

        #region Constructor

        public CartItemController(IProductService productService, IProductVariantService productVariantService, ICartItemService cartItemService, IOrderService orderService)
        {
            _productService = productService;
            _productVariantService = productVariantService;
            _cartItemService = cartItemService;
            _orderService = orderService;
        }

        #endregion

        #region Views

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult OrderConfirmation(int orderId)
        {
            var order = _orderService.GetOrderById(orderId); 

            if (order == null)
                return NotFound("Order not found.");

            var confirmation = _orderService.ConfirmOrder(order);

            return View(confirmation); 
        }

        #endregion

        #region Methods

        [HttpGet]
        public IActionResult GetSessionId()
        {
            return Json(new { sessionId = HttpContext.Session.Id });
        }

        [HttpGet]
        public IActionResult GetAllCartItems(string sessionId)
        {
            var cartItems = _cartItemService.GetListOfCartItems(sessionId);
            var result = cartItems.Select(item => new
            {
                id = item.Id,
                quantity = item.Quantity,
                productId = item.ProductId,
                productVariantId = item.ProductVariantId,
                productName = item.Product.Name,
                productPrice = item.Product.Price,
                productSize = item.ProductVariant.Size,
                variantStock = item.ProductVariant.Stock,
                productImageUrl = item.Product.ProductImages.FirstOrDefault()?.Url,
            }).ToList();

            return Json(result);
        }

        [HttpGet]
        public IActionResult GetCartItem(int id)
        {
            var cartItem = _cartItemService.GetCartItemById(id);
            if (cartItem == null)
                return NotFound();

            return Ok(cartItem);
        }

        [HttpGet]
        public IActionResult GetQuantityInCart(int productVariantId)
        {
            var sessionId = HttpContext.Session.Id;
            var quantityInCart = _cartItemService.GetQuantityInCart(sessionId, productVariantId);
            return Json(quantityInCart);
        }

        [HttpPost]
        public IActionResult AddToCart([FromForm] int productVariantId, [FromForm] int productId, [FromForm] int quantity)
        {
            if (quantity < 1)
                return BadRequest("Quantity must be at least 1.");

            string sessionId = HttpContext.Session.Id;

            var variant = _productVariantService.GetProductVariant(productVariantId);
            if (variant == null || variant.Stock < quantity)
                return BadRequest("Variant not available or insufficient stock.");

            _cartItemService.AddCartItem(sessionId, productId, productVariantId, quantity);

            return Ok(new { message = "Item added to cart." });
        }

        [HttpPost]
        public IActionResult UpdateQuantity([FromForm] int cartItemId, [FromForm] int quantity)
        {
            if (quantity < 1)
                return BadRequest(new { message = "Quantity must be at least 1." });

            _cartItemService.UpdateCartItem(cartItemId, quantity, null);

            return Ok(new { message = "Quantity updated." });
        }

        [HttpPost]
        public IActionResult RemoveFromCart([FromForm] int cartItemId)
        {
            _cartItemService.DeleteCartItem(cartItemId);
           
            return Ok(new { message = "Item removed from cart." });
        }

        [HttpPost]
        public IActionResult Checkout(string firstName, string lastName, string governate, string instagramAccount, string phoneNumber, string fullAddress, decimal totalPrice)
        {
            var sessionId = HttpContext.Session.Id;

            if (string.IsNullOrEmpty(sessionId))
            {
                return BadRequest("Session expired.");
            }

            var order = _orderService.AddOrder(sessionId, firstName, lastName, governate, instagramAccount, phoneNumber, fullAddress, totalPrice);

            return Ok(new { orderId = order.Id }); 
        }

        #endregion
    }
}
