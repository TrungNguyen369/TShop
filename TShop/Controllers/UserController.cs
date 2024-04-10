using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TShop.IServices;
using TShop.Models;
using TShop.Services;
using TShop.ViewComponents;
using TShop.ViewModels;

namespace TShop.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly TshopContext _context;

        public UserController(TshopContext context, IUserService userService)
        {
            _userService = userService;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RegisterUser(UserVM userVM, bool gender)
        {
            if (ModelState.IsValid)
            {
                _userService.UserRegister(userVM, gender);

                return RedirectToAction("Index", "Product");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {             
                var userVM = _userService.UserLogin(loginVM);
                if (userVM != null)
                {
                    return RedirectToAction("Index", "Product");
                }
            }

            return RedirectToAction("Index");
        }
    }
}
