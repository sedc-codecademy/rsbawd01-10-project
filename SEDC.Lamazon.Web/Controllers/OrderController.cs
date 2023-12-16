using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SEDC.Lamazon.Services.Interfaces;
using SEDC.Lamazon.Services.ViewModels.Order;
using System.Security.Claims;

namespace SEDC.Lamazon.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShoppingCart() 
        {
            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString);

            OrderViewModel activeOrderData = _orderService.GetActiveOrder(userId);
        
            return View(activeOrderData);
        }

        public IActionResult Summary() 
        {
            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString);

            OrderViewModel activeOrderData = _orderService.GetActiveOrder(userId);

            return View(activeOrderData);
        }

        [HttpPost]
        public IActionResult SummaryPost(OrderViewModel model)
        {
            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString);

            OrderViewModel response = _orderService.SubmitOrder(model);

            // Add Stripe (payment) option

            return View("Confirmation", response);
        }
    }
}
