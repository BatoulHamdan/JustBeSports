using JustBeSports.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JustBeSportsAdmin.Controllers
{
    public class OrderController : AdminBaseController
    {
        #region Properties

        private readonly IOrderService _orderService;

        #endregion

        #region Constructor

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #endregion

        #region Views

        public IActionResult Index()
        {
            return View();
        }

        #endregion

        #region Methods

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var orders = _orderService.GetListOfOrders();
            return Json(orders);
        }

        [HttpGet("Order/ViewOrder/{orderId}")]
        public IActionResult ViewOrder(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public IActionResult UpdateOrderStatus(int orderId, string newStatus)
        {
            _orderService.UpdateOrderStatus(orderId, newStatus);
            return Ok();
        }

        [HttpPost]
        public IActionResult DeleteOrder(int orderId)
        {
            try
            {
                _orderService.DeleteOrder(orderId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the order.");
            }
        }

        #endregion
    }
}
