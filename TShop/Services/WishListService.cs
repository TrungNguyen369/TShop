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

        /// <summary>
        /// Handle add wishlist
        /// </summary>
        /// <param name="wishListItems"></param>
        /// <param name="idProduct"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public List<WishItem> AddWishListItem(List<WishItem> wishListItems, int idProduct, int quantity)
        {
            //Find item exsist cart?
            var item = wishListItems.Find(x => x.Id == idProduct);

            //New item add cart
            //Defaut add quantity in item in cart
            if (item == null)
            {
                //Get product item
                var product = _context.Products.FirstOrDefault(x => x.IdProduct == idProduct);

                //Define CartItem from product 
                item = new WishItem
                {
                    Id = idProduct,
                    Quantity = quantity,
                    Name = product.NameProduct,
                    Price = (int)product.Price,
                    image = product.Image
                };

                //Add item in wishlist
                wishListItems.Add(item);
            }
            else
            {
                //Add quantity item ready in wishlist
                item.Quantity += quantity;
            }

            return wishListItems;
        }

        /// <summary>
        /// Decease quantity item ready in wishlist
        /// </summary>
        /// <param name="wishListItems"></param>
        /// <param name="idProduct"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
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
                    //Reduce quantity item wishlist
                    item.Quantity -= quantity;
                }
            }

            return wishListItems;
        }

        /// <summary>
        /// Hanlde delete item in wishlist
        /// </summary>
        /// <param name="wishListItems"></param>
        /// <param name="idProduct"></param>
        /// <returns></returns>
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
