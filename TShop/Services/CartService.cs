using System.Collections.Generic;
using TShop.Contants;
using TShop.IServices;
using TShop.Models;
using TShop.ViewModels;

namespace TShop.Services
{
    public class CartService : ICartService
    {
        private readonly TshopContext _context;

        public CartService(TshopContext context)
        {
            _context = context;
        }
        public List<CartItem> AddProductToCart(List<CartItem> cartItems, int idProduct, int quantity)
        {
            //find item exsist cart?
            var item = cartItems.Find(x => x.Id == idProduct);

            //new item add cart
            //defaut add quantity in item in cart
            if (item == null)
            {
                //get product item
                var product = _context.Products.FirstOrDefault(x => x.IdProduct == idProduct);

                //define CartItem from product 
                item = new CartItem
                {
                    Id = idProduct,
                    Quantity = quantity,
                    Name = product.NameProduct,
                    Price = (int)product.Price,
                    image = product.Image
                };

                //add cart
                cartItems.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }

            return cartItems;
        }

        public List<CartItem> GetCartItems()
        {
            throw new NotImplementedException();
        }

        public List<CartItem> ReduceQuantityProduct(List<CartItem> cartItems, int idProduct, int quantity)
        {
            //find item exsist cart?
            var item = cartItems.Find(x => x.Id == idProduct);

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

            return cartItems;
        }

        public List<CartItem> RevomeProductToCart(List<CartItem> cartItems, int idProduct)
        {
            //find item exsist cart?
            var item = cartItems.Find(x => x.Id == idProduct);

            //check item add cart
            if (item != null)
            {
                //Revome cart
                cartItems.Remove(item);
            }

            return cartItems;
        }
    }
}
