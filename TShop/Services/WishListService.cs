using Microsoft.EntityFrameworkCore;
using TShop.IServices;
using TShop.Models;
using TShop.ViewModels;

namespace TShop.Services
{
    public class WishListService : IWishListService
    {
        private readonly TshopContext _context;

        public WishListService(TshopContext context) 
        { 
            _context = context; 
        }

        public List<WishItem> AddWishListItem(List<WishItem> wishListItems, int idProduct, int quantity)
        {
            //find item exsist cart?
            var item = wishListItems.Find(x => x.Id == idProduct);

            //new item add cart
            //defaut add quantity in item in cart
            if (item == null)
            {
                //get product item
                var product = _context.Products.FirstOrDefault(x => x.IdProduct == idProduct);

                //define CartItem from product 
                item = new WishItem
                {
                    Id = idProduct,
                    Quantity = quantity,
                    Name = product.NameProduct,
                    Price = (int)product.Price,
                    image = product.Image
                };

                //add cart
                wishListItems.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }

            return wishListItems;
        }

        public List<WishItem> ReduceWishListItem(List<WishItem> wishListItems, int idProduct, int quantity)
        {
            //find item exsist cart?
            var item = wishListItems.Find(x => x.Id == idProduct);

            //check item reduce cart
            if (item != null)
            {
                if (item.Quantity <= 1)
                {
                    item.Quantity = 0;
                }
                else
                {
                    //Reduce cart
                    item.Quantity -= quantity;
                }
            }

            return wishListItems;
        }

        public List<WishItem> RevomeWishListItem(List<WishItem> wishListItems, int idProduct)
        {
            //find item exsist cart?
            var item = wishListItems.Find(x => x.Id == idProduct);

            //check item add cart
            if (item != null)
            {
                //Revome cart
                wishListItems.Remove(item);
            }

            return wishListItems;
        }
    }
}
