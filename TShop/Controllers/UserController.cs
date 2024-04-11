using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TShop.Contants;
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

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var userVM = await _userService.UserLoginAsync(loginVM);
                if (userVM != null)
                {
                    var claims = new List<Claim> {
                                new Claim(ClaimTypes.Email, userVM.Email),
                                new Claim(ClaimTypes.Name, userVM.Name),
                                
                                //Role
                                new Claim(ClaimTypes.Role, Constants.CLAIM_CUSTOMER)
                            };

                    var claimsIndentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincical = new ClaimsPrincipal(claimsIndentity);

                    await HttpContext.SignInAsync(claimsPrincical);

                    return RedirectToAction("AccountProfile");
                }
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult AccountProfile()
        {
            return View();
        }
    }
}
