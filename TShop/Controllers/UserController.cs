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

        /// <summary>
        /// Handle user registration
        /// </summary>
        /// <param name="userVM">user view model</param>
        /// <param name="gender">0:male and 1: female</param>
        /// <returns></returns>
        public IActionResult RegisterUser(UserVM userVM, bool gender)
        {
            //Check if the model is vaild
            if (ModelState.IsValid)
            {
                //Handle register for user
                _userService.UserRegister(userVM, gender);

                return RedirectToAction(Constants.INDEX, Constants.PRODUCT);
            }
            return RedirectToAction(Constants.INDEX);
        }

        /// <summary>
        /// Get data in login page
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return RedirectToAction(Constants.INDEX);
        }

        /// <summary>
        /// Handle login of user
        /// </summary>
        /// <param name="loginVM"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            //Check if the model is vaild
            if (ModelState.IsValid)
            {
                //Handle user login
                var userVM = await _userService.UserLoginAsync(loginVM);

                //Defide cliam
                if (userVM != null)
                {
                    var claims = new List<Claim> {
                                new Claim(ClaimTypes.Email, userVM.Email),
                                new Claim(ClaimTypes.Name, userVM.Name),
                                
                                //Role
                                new Claim(ClaimTypes.Role, Constants.CLAIM_CUSTOMER)
                            };
                    //Create new Identity
                    var claimsIndentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincical = new ClaimsPrincipal(claimsIndentity);

                    //Sign claim
                    await HttpContext.SignInAsync(claimsPrincical);

                    //Define model return
                    var shareModel = new SharedViewModel
                    {
                        UserVM = userVM,
                    };

                    return RedirectToAction(Constants.ACCOUNTPROFILE, shareModel);
                }
            }

            return RedirectToAction(Constants.INDEX);
        }

        /// <summary>
        /// Sign out of user current accuont
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction(Constants.INDEX);
        }

        /// <summary>
        /// The page Accuont profile
        /// </summary>
        /// <param name="shareModel">Data processing: users and passwords</param>
        /// <returns></returns>
        [Authorize]
        public IActionResult AccountProfile(SharedViewModel shareModel)
        {
            return View(shareModel);
        }

        /// <summary>
        /// Handle request change password user
        /// </summary>
        /// <param name="passwordVM">passwork model</param>
        /// <param name="emailUser">email of user</param>
        /// <returns></returns>
        [Authorize]
        public IActionResult PasswordChange(PasswordVM passwordVM, string emailUser)
        {
            //Check that the input data must be different from null
            if (emailUser == null)
            {
                return RedirectToAction(Constants.ACCOUNTPROFILE);
            }

            //The password cannot be empty
            //The old password cannot be equal to the new password
            //The new passwork muse be equal to the comfimation password
            if (!passwordVM.NewPassword.Equals("") || passwordVM.NewPassword != passwordVM.ConfirmPassword || passwordVM.CurrentPassword == passwordVM.NewPassword)
            {
                return RedirectToAction(Constants.ACCOUNTPROFILE);
            }

            //Handle password change
            var user = _userService.UserPasswordChange(passwordVM, emailUser);

            //Define share model return result
            var shareModel = new SharedViewModel
            {
                UserVM = user
            };

            return RedirectToAction(Constants.ACCOUNTPROFILE, shareModel);
        }
    }
}
