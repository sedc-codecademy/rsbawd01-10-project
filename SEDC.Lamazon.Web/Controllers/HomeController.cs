using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SEDC.Lamazon.Services.Interfaces;
using SEDC.Lamazon.Services.ViewModels.Order;
using SEDC.Lamazon.Services.ViewModels.Product;
using SEDC.Lamazon.Web.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace SEDC.Lamazon.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;

        public HomeController(ILogger<HomeController> logger, 
            IProductService productService, 
            IOrderService orderService, 
            IOrderItemService orderItemService)
        {
            _logger = logger;
            _productService = productService;
            _orderService = orderService;
            _orderItemService = orderItemService;
        }

        public IActionResult Index()
        {
            List<ProductViewModel> products = _productService.GetAllProducts();
            return View(products);
        }

        public IActionResult ProductDetails(int id) 
        {
            ProductViewModel productDetails = _productService.GetProductById(id);

            return View(productDetails);
        }

        [Authorize]
        public IActionResult AddToCart(int productId) 
        {
            try
            {
                string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userId = int.Parse(userIdString);

                OrderViewModel activeOrder = _orderService.GetActiveOrder(userId);

                if (activeOrder == null)
                {
                    OrderViewModel orderViewModel = new OrderViewModel()
                    {
                        UserId = userId,
                    };

                    _orderService.CreateOrder(orderViewModel);

                    activeOrder = _orderService.GetActiveOrder(userId);
                }

                _orderItemService.CreateOrderItem(productId, activeOrder.Id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log exception
                return View("Error");
            }
        }

        [Authorize]
        public IActionResult UserInfo() 
        {
            return View();
        }
    }
}