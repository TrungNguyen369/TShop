using Microsoft.AspNetCore.Mvc;
using TShop.Helpers;
using TShop.Contants;
using TShop.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TShop.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var carts = HttpContext.Session.Get<List<CartItem>>(Constants.CART_KEY) ?? new List<CartItem>();

            var reslut = new CartPayLoad
            {
                Quantity = carts.Sum(x => x.Quantity),
            };

            return View(reslut);
        }
    }
}
