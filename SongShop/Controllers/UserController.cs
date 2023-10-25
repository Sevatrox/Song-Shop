using DemoAudit.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SongShop.DataAccess.Repository.IRepository;
using SongShop.Models;
using SongShop.Models.ViewModels;
using System.Security.Claims;

namespace SongShop.Controllers
{
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
		private readonly ILogger<UserController> _logger;

		public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }
        public IActionResult Index()
        {
			_logger.LogInformation("Index method of UserController started!");
			_logger.LogInformation("Index method of UserController ended!");
			return View();
        }

        public IActionResult Register()
        {
			_logger.LogInformation("Register method of UserController started!");
			_logger.LogInformation("Register method of UserController ended!");
			return View();
        }

        [HttpPost]
        public IActionResult Register(User userObj)
        {
			_logger.LogInformation("Register POST method of UserController started!");
			if (ModelState.IsValid)
            {
                if (_userRepository.CheckEmail(userObj.Email))
                {
                    TempData["error"] = "User with the same email already exists!";
					_logger.LogInformation("Register POST method of UserController ended!");
					return RedirectToAction("Register", "User");
                }

                _userRepository.AddUser(userObj);

                TempData["success"] = "User created!";
            }
			_logger.LogInformation("Register POST method of UserController ended!");
			return RedirectToAction("Index", "Home");
		}

        public IActionResult Login()
        {
			_logger.LogInformation("Login method of UserController started!");
			_logger.LogInformation("Login method of UserController ended!");
			return View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginVM userObj)
        {
			_logger.LogInformation("Login POST method of UserController started!");

			User userFromDb = _userRepository.Login(userObj);
            if (userFromDb != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userFromDb.Email),
                    new Claim(ClaimTypes.Role, userFromDb.UserType),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties());

                if(userFromDb.UserType == "Seller")
                {
					TempData["success"] = "User logged in!";
					_logger.LogInformation("Login POST method of UserController ended!");
					return RedirectToAction("Index", "Home");
				}
                else if(userObj.SongId != null)
                {
					_logger.LogInformation("Login POST method of UserController ended!");
					return RedirectToAction("BuySong", "Song", new {id = userObj.SongId});
				}

				TempData["success"] = "User logged in!";
				_logger.LogInformation("Login POST method of UserController ended!");
				return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = "User does not exist!";
				_logger.LogInformation("Login POST method of UserController ended!");
				return RedirectToAction("Login", "User");
            }

        }

        public IActionResult Logout()
        {
			_logger.LogInformation("Logout method of UserController started!");

			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            TempData["success"] = "User logged out!";
			_logger.LogInformation("Logout method of UserController ended!");
			return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Edit()
        {
			_logger.LogInformation("Edit method of UserController started!");

			string userEmail = User.Claims.First().Value;
            User userFromDb = _userRepository.Get(userEmail);

			_logger.LogInformation("Edit method of UserController ended!");
			return View(userFromDb);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(User userObj)
        {
			_logger.LogInformation("Edit POST method of UserController started!");
			_userRepository.UpdateUser(userObj);

            TempData["success"] = "User updated!";
			_logger.LogInformation("Edit POST method of UserController ended!");
			return RedirectToAction("Index", "Home");
        }

    }
}
