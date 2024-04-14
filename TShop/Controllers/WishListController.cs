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

        /// <summary>
        /// Add WishList 
        /// </summary>
        /// <param name="id">id of product</param>
        /// <param name="quantity">quantity of product</param>
        /// <returns></returns>
        public IActionResult AddWishList(int id, int quantity = 1)
        {
            var wishListItems = WishList;

            //Add item item into wishlist 
            wishListItems = _wishListService.AddWishListItem(wishListItems, id, quantity);

            //Save item
            HttpContext.Session.Set(Constants.CART_KEY, wishListItems);

            return RedirectToAction(Constants.INDEX);
        }

        /// <summary>
        /// Add WishList
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddWishList([FromBody] CartPayLoad data)
        {
            var wishListItems = WishList;

            //Get id and quantity from product post
            var quantity = data.Quantity;
            var id = data.Id;

            //Add item into wish list 
            wishListItems = _wishListService.AddWishListItem(wishListItems, id, quantity);

            //Save wish list
            HttpContext.Session.Set(Constants.CART_KEY, wishListItems);

            return Ok();
        }

        /// <summary>
        /// Reduce item already in Wishlist
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ReduceWishList([FromBody] CartPayLoad data)
        {
            var wishItems = WishList;

            //Get id and quantity from product post
            var quantity = data.Quantity;
            var id = data.Id;

            //Add item into wishItems 
            wishItems = _wishListService.ReduceWishListItem(wishItems, id, quantity);

            //Save wishItems
            HttpContext.Session.Set(Constants.CART_KEY, wishItems);

            return Ok();
        }

        /// <summary>
        /// Delete item is ready in wishlist
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IActionResult RemoveWishList([FromBody] CartPayLoad data)
        {
            var wishItems = WishList;

            var id = data.Id;

            //Add item into wishlist 
            wishItems = _wishListService.RevomeWishListItem(wishItems, id);

            //Save wishlist
            HttpContext.Session.Set(Constants.CART_KEY, wishItems);

            return Ok();
        }
    }
}
