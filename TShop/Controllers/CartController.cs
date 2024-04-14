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

        /// <summary>
        /// Add items to cart
        /// </summary>
        /// <param name="id">Id of product</param>
        /// <param name="quantity">Quantity default is one</param>
        /// <returns>Move the quest back to Index</returns>
        public IActionResult AddCart(int id, int quantity = 1)
        {
            var cartItems = Cart;

            //add cart item into cart 
            cartItems = _cartService.AddProductToCart(cartItems, id, quantity);

            //save cart
            HttpContext.Session.Set(Constants.CART_KEY, cartItems);

            return RedirectToAction(Constants.INDEX);
        }

        /// <summary>
        /// Add cart by payload
        /// </summary>
        /// <param name="data">Data includes product id and product quantity/param>
        /// <returns>Move the quest back to Index</returns>
        [HttpPost]
        public IActionResult AddCart([FromBody] CartPayLoad data)
        {
            var cartItems = Cart;

            //Get id and quantity from product post
            var quantity = data.Quantity;
            var id = data.Id;

            //Add cart item into cart 
            cartItems = _cartService.AddProductToCart(cartItems, id, quantity);

            //Save cart into Session
            HttpContext.Session.Set(Constants.CART_KEY, cartItems);

            return RedirectToAction(Constants.INDEX);
        }

        /// <summary>
        /// Reduce the number of items already in the cart
        /// </summary>
        /// <param name="data">Data includes product id and product quantity</param>
        /// <returns>Move the quest back to Index</returns>
        [HttpPost]
        public IActionResult ReduceCart([FromBody] CartPayLoad data)
        {
            var cartItems = Cart;

            //Get id and quantity from product post
            var quantity = data.Quantity;
            var id = data.Id;

            //Add cart item into cart 
            cartItems = _cartService.ReduceQuantityProduct(cartItems, id, quantity);

            //Save Session cart
            HttpContext.Session.Set(Constants.CART_KEY, cartItems);

            return RedirectToAction(Constants.INDEX);
        }

        /// <summary>
        /// Remove existing items from the cart
        /// </summary>
        /// <param name="data">Data includes product id and product quantity</param>
        /// <returns>Move the quest back to Index</returns>
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

        /// <summary>
        /// Check Out
        /// </summary>
        /// <returns>Page check out</returns>
        [Authorize]
        [HttpGet]
        public IActionResult CheckOut()
        {
            //If the cart does not contain any items, please return to the previous page
            if (Cart.Count() == 0)
            {
                return Redirect("/");
            }

            //Get email by cookie and If the email does not contain any items, please return to the previous page
            var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Contains(Constants.EMAILADDRESS)).Value;
            if (email == null)
            {
                return Redirect("/");
            }

            ////Get user by email and If the user does not contain any items, please return to the previous page
            var user = _userService.GetUserByEmail(email);
            if (user == null)
            {
                return Redirect("/");
            }

            //Define checkout view model
            var checkOutVM = new CheckOutVM
            {
                cartItems = Cart,
                IdUser = user.IdCustomer,
                Email = email,
                Phone = user.Phone,
                Address = user.Address,
                UserName = user.FullName,
            };

            //Save user into Seccion
            HttpContext.Session.Set(Constants.CHECKOUT_KEY, checkOutVM);
          
            ViewBag.PayPalClientId = _paypalClient.ClientId;

            return View(checkOutVM);
        }

        /// <summary>
        /// Handle checkout tasks
        /// </summary>
        /// <param name="payment">This is payment method</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IActionResult CheckOut(string? payment)
        {
            //Get check out view model in Seccion
            var checkOutVM = HttpContext.Session.Get<CheckOutVM>(Constants.CHECKOUT_KEY);

            //Payment method is VNpay
            if (payment == Constants.VNPAY)
            {
                //Define VNPay request model
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

            //Handle checkout
            var result = _cartService.CheckOut(checkOutVM, payment, Cart);

            //If the result is success, reset the cart
            if (result == Constants.SUCCESS)
            {
                var cartItems = new List<CartItem>();
                HttpContext.Session.Set(Constants.CART_KEY, cartItems);
            }

            return View(result);
        }

        #region PaypalClient
        /// <summary>
        /// Create Paypal Order
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
                //Handel create order
                var response = await _paypalClient.CreateOrder(grandTotal, currencyUnit, ReferenceOrderCode);

                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }

        /// <summary>
        /// Capture Paypal orders
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Cart/capture-paypal-order")]
        public async Task<IActionResult> CapturePaypalOrder(string orderID, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderID);

                //Get data cart in secction
                var value = HttpContext.Session.Get<CheckOutVM>(Constants.CHECKOUT_KEY);

                //Handle checkout and save database
                var result = _cartService.CheckOut(value, Constants.PAYPAL, Cart);

                //If the reslt is success, reset the cart 
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

        /// <summary>
        /// This is the function that processes VNpay after creating a payment
        /// </summary>
        /// <returns>Page success or Fail</returns>
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
