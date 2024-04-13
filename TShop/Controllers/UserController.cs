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

                return RedirectToAction(Constants.INDEX, Constants.PRODUCT);
            }
            return RedirectToAction(Constants.INDEX);
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return RedirectToAction(Constants.INDEX);
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

                    var shareModel = new SharedViewModel
                    {
                        UserVM = userVM,
                    };

                    return RedirectToAction(Constants.ACCOUNTPROFILE, shareModel);
                }
            }

            return RedirectToAction(Constants.INDEX);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction(Constants.INDEX);
        }

        [Authorize]
        public IActionResult AccountProfile(SharedViewModel shareModel)
        {
            return View(shareModel);
        }

        [Authorize]
        public IActionResult PasswordChange(PasswordVM passwordVM, string emailUser)
        {
            if (emailUser == null)
            {
                return RedirectToAction(Constants.ACCOUNTPROFILE);
            }

            if (!passwordVM.NewPassword.Equals("") || passwordVM.NewPassword != passwordVM.ConfirmPassword || passwordVM.CurrentPassword == passwordVM.NewPassword)
            {
                return RedirectToAction(Constants.ACCOUNTPROFILE);
            }

            var user = _userService.UserPasswordChange(passwordVM, emailUser);

            var shareModel = new SharedViewModel
            {
                UserVM = user
            };

            return RedirectToAction(Constants.ACCOUNTPROFILE, shareModel);
        }
    }
}
