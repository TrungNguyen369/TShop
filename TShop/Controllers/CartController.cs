using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Reflection.Metadata;
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

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
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

            return RedirectToAction("Index");
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

            return RedirectToAction("Index");
        }
    }
}
