using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TShop.Contants;
using TShop.Helpers;
using TShop.IServices;
using TShop.Services;
using TShop.ViewModels;

namespace TShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly PaypalClient _paypalClient;
        private readonly IUserService _userService;
        private readonly IVnPayService _vnPayService;

        public CartController(ICartService cartService, PaypalClient paypalClient, IUserService userService, IVnPayService vnPayService)
        {
            _cartService = cartService;
            _paypalClient = paypalClient;
            _userService = userService;
            _vnPayService = vnPayService;
        }

        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(Constants.CART_KEY) ?? new List<CartItem>();

        public IActionResult Index()
        {
            return View(Cart);
        }

        public IActionResult AddCart(int id, int quantity = 1)
        {
            var cartItems = Cart;

            //add cart item into cart 
            cartItems = _cartService.AddProductToCart(cartItems, id, quantity);

            //save cart
            HttpContext.Session.Set(Constants.CART_KEY, cartItems);

            return RedirectToAction(Constants.INDEX);
        }

        [HttpPost]
        public IActionResult AddCart([FromBody] CartPayLoad data)
        {
            var cartItems = Cart;

            //get id and quantity from product post
            var quantity = data.Quantity;
            var id = data.Id;

            //add cart item into cart 
            cartItems = _cartService.AddProductToCart(cartItems, id, quantity);

            //save cart
            HttpContext.Session.Set(Constants.CART_KEY, cartItems);

            return RedirectToAction(Constants.INDEX);
        }

        [HttpPost]
        public IActionResult ReduceCart([FromBody] CartPayLoad data)
        {
            var cartItems = Cart;

            //get id and quantity from product post
            var quantity = data.Quantity;
            var id = data.Id;

            //Add cart item into cart 
            cartItems = _cartService.ReduceQuantityProduct(cartItems, id, quantity);

            //Save Session cart
            HttpContext.Session.Set(Constants.CART_KEY, cartItems);

            return RedirectToAction(Constants.INDEX);
        }

        [HttpPost]
        public IActionResult RemoveCart([FromBody] CartPayLoad data)
        {
            var cartItems = Cart;

            var id = data.Id;

            //Add cart item into cart 
            cartItems = _cartService.RevomeProductToCart(cartItems, id);

            //Save Session cart
            HttpContext.Session.Set(Constants.CART_KEY, cartItems);

            return RedirectToAction(Constants.INDEX);
        }

        [Authorize]
        [HttpGet]
        public IActionResult CheckOut()
        {
            if (Cart.Count() == 0)
            {
                return Redirect("/");
            }

            var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Contains(Constants.EMAILADDRESS)).Value;
            if (email == null)
            {
                return Redirect("/");
            }

            var user = _userService.GetUserByEmail(email);
            if (user == null)
            {
                return Redirect("/");
            }

            var checkOutVM = new CheckOutVM
            {
                cartItems = Cart,
                IdUser = user.IdCustomer,
                Email = email,
                Phone = user.Phone,
                Address = user.Address,
                UserName = user.FullName,
            };

            HttpContext.Session.Set(Constants.CHECKOUT_KEY, checkOutVM);

            ViewBag.PayPalClientId = _paypalClient.ClientId;

            return View(checkOutVM);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CheckOut(string? payment)
        {
            var checkOutVM = HttpContext.Session.Get<CheckOutVM>(Constants.CHECKOUT_KEY);

            if (payment == Constants.VNPAY)
            {
                var vnPayModel = new VnPaymentRequestModel
                {
                    Amount = checkOutVM.GrandTotal,
                    CreatedDate = DateTime.Now,
                    Description = $"{checkOutVM.Address} {checkOutVM.Phone}",
                    FullName = checkOutVM.UserName,
                    OrderId = new Random().Next(1000, 10000)
                };
                return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
            }

            var result = _cartService.CheckOut(checkOutVM, payment, Cart);

            if (result == Constants.SUCCESS)
            {
                var cartItems = new List<CartItem>();
                HttpContext.Session.Set(Constants.CART_KEY, cartItems);
            }

            return View(result);
        }

        [Authorize]
        public IActionResult PaymentSuccess()
        {
            return View(Constants.SUCCESS);
        }

        #region PaypalClient
        [Authorize]
        [HttpPost("/Cart/create-paypal-order")]
        public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken)
        {
            //Order information sent via Paypal
            var grandTotal = Cart.Sum(x => x.TotalPrice).ToString();
            var currencyUnit = Constants.CURRENCY_USD;
            var ReferenceOrderCode = Constants.ORDER_CODE + DateTime.Now.Ticks.ToString();

            try
            {
                var response = await _paypalClient.CreateOrder(grandTotal, currencyUnit, ReferenceOrderCode);

                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }

        [Authorize]
        [HttpPost("Cart/capture-paypal-order")]
        public async Task<IActionResult> CapturePaypalOrder(string orderID, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderID);

                //Save database
                var value = HttpContext.Session.Get<CheckOutVM>(Constants.CHECKOUT_KEY);

                var result = _cartService.CheckOut(value, Constants.PAYPAL, Cart);

                if (result == Constants.SUCCESS)
                {
                    var cartItems = new List<CartItem>();
                    HttpContext.Session.Set(Constants.CART_KEY, cartItems);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }
        #endregion

        [Authorize]
        public IActionResult PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != Constants.VNPAY_RESPRONSE_CODE)
            {
                return View(Constants.FAIL);
            }

            //Save database
            var value = HttpContext.Session.Get<CheckOutVM>(Constants.CHECKOUT_KEY);

            var result = _cartService.CheckOut(value, Constants.VNPAY, Cart);

            if (result == Constants.SUCCESS)
            {
                var cartItems = new List<CartItem>();
                HttpContext.Session.Set(Constants.CART_KEY, cartItems);
            }

            return View(Constants.SUCCESS);
        }
    }
}
