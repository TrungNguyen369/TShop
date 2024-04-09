using Microsoft.AspNetCore.Mvc;
using TShop.Contants;
using TShop.Helpers;
using TShop.IServices;
using TShop.Services;
using TShop.ViewModels;

namespace TShop.Controllers
{
    public class WishListController : Controller
    {
        private readonly IWishListService _wishListService;

        public List<WishItem> WishList => HttpContext.Session.Get<List<WishItem>>(Constants.CART_KEY) ?? new List<WishItem>();

        public WishListController(IWishListService wishListService) 
        {
            _wishListService = wishListService;
        }

        public IActionResult Index()
        {
            return View(WishList);
        }

        public IActionResult AddWishList(int id, int quantity = 1)
        {
            var wishListItems = WishList;

            //add cart item into cart 
            wishListItems = _wishListService.AddWishListItem(wishListItems, id, quantity);

            //save cart
            HttpContext.Session.Set(Constants.CART_KEY, wishListItems);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddWishList([FromBody] CartPayLoad data)
        {
            var wishListItems = WishList;

            //get id and quantity from product post
            var quantity = data.Quantity;
            var id = data.Id;

            //add cart item into wish list 
            wishListItems = _wishListService.AddWishListItem(wishListItems, id, quantity);

            //save wish list
            HttpContext.Session.Set(Constants.CART_KEY, wishListItems);

            return Ok();
        }

        [HttpPost]
        public IActionResult ReduceWishList([FromBody] CartPayLoad data)
        {
            var wishItems = WishList;

            //get id and quantity from product post
            var quantity = data.Quantity;
            var id = data.Id;

            //add cart item into wishItems 
            wishItems = _wishListService.ReduceWishListItem(wishItems, id, quantity);

            //save wishItems
            HttpContext.Session.Set(Constants.CART_KEY, wishItems);

            return Ok();
        }

        public IActionResult RemoveWishList([FromBody] CartPayLoad data)
        {
            var wishItems = WishList;

            var id = data.Id;

            //add cart item into cart 
            wishItems = _wishListService.RevomeWishListItem(wishItems, id);

            //save cart
            HttpContext.Session.Set(Constants.CART_KEY, wishItems);

            return Ok();
        }
    }
}
