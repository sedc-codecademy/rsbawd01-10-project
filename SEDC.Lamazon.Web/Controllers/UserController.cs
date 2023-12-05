using Microsoft.AspNetCore.Mvc;
using SEDC.Lamazon.Services.Interfaces;
using SEDC.Lamazon.Services.ViewModels.User;

namespace SEDC.Lamazon.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Register()
        {
            RegisterUserViewModel model = new RegisterUserViewModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegisterUserViewModel model) 
        {
            try
            {
                if (model == null)
                    return BadRequest();

                // TODO: We will fix this later
                model.RoleId = 2;

                // Pass the model to registration service method
                _userService.RegisterUser(model);

                return View("SuccessRegistration");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
    }
}
