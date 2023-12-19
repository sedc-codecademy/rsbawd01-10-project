using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SEDC.Lamazon.Services.Interfaces;
using SEDC.Lamazon.Services.ViewModels.Order;
using SEDC.Lamazon.Services.ViewModels.OrderItem;
using Stripe;
using Stripe.Checkout;
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

            // TODO FIX THIS
            OrderViewModel activeOrderData = _orderService.GetActiveOrder(userId);

            // Add Stripe (payment) option

            string domain = "https://localhost:7128";

            SessionCreateOptions sessionCreateOptions = new SessionCreateOptions()
            {
                SuccessUrl = $"{domain}/Order/Confirmation?orderNumber={model.OrderNumber}",
                CancelUrl = $"{domain}/Order/ShoppingCart",
                Mode = "payment",
                LineItems = new List<SessionLineItemOptions>()
            };

            foreach (OrderItemViewModel orderItem in activeOrderData.Items)
            {
                SessionLineItemOptions sessionLineItem = new SessionLineItemOptions()
                { 
                    Quantity = orderItem.Qty,
                    PriceData = new SessionLineItemPriceDataOptions() 
                    { 
                        UnitAmount = (long)(orderItem.Price * 100),
                        Currency = "eur",
                        ProductData = new SessionLineItemPriceDataProductDataOptions() 
                        { 
                            Name = orderItem.Product.Name
                        }
                    }
                };

                sessionCreateOptions.LineItems.Add(sessionLineItem);
            }

            SessionService service = new SessionService();
            Session session = service.Create(sessionCreateOptions);
           
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public IActionResult Confirmation(string orderNumber)
        {
            ViewData["orderNumber"] = orderNumber;
            return View();
        }
    }
}
