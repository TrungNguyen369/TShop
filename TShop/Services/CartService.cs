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
        private readonly IUserService _userService;

        public CartService(TshopContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        /// <summary>
        /// Handle add product into carts
        /// </summary>
        /// <param name="cartItems"></param>
        /// <param name="idProduct"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public List<CartItem> AddProductToCart(List<CartItem> cartItems, int idProduct, int quantity)
        {
            //Find item exsist cart?
            var item = cartItems.Find(x => x.IdProduct == idProduct);

            //New item add cart
            //Defaut add quantity in item in cart
            if (item == null)
            {
                //get product item
                var product = _context.Products.FirstOrDefault(x => x.IdProduct == idProduct);

                //define CartItem from product 
                item = new CartItem
                {
                    IdProduct = idProduct,
                    Quantity = quantity,
                    Name = product.NameProduct,
                    Price = (int)product.Price,
                    image = product.Image
                };

                //Add new item into cart
                cartItems.Add(item);
            }
            else
            {
                //Add quantity item ready in cart
                item.Quantity += quantity;
            }

            return cartItems;
        }

        /// <summary>
        /// Handle checkout
        /// </summary>
        /// <param name="checkOutVM"></param>
        /// <param name="payment"></param>
        /// <param name="cartItems"></param>
        /// <returns></returns>
        public string CheckOut(CheckOutVM checkOutVM, string? payment, List<CartItem> cartItems)
        {
            //Define reslut return
            var result = Constants.FAIL;

            //Get user by email and check user exist
            var user = _userService.GetUserByEmail(checkOutVM.Email);
            if (user == null)
            {
                return result;
            }

            //Create bill prepare in save datebase
            var bill = new Invoice
            {
                OrderDate = DateTime.Now,
                Name = checkOutVM.UserName,
                Address = checkOutVM.Address,
                Phone = checkOutVM.Phone,
                PaymentMethod = payment ?? "COD",
                StatusCode = 1,
                IdCustomer = user.IdCustomer.ToString(),
            };

            _context.Database.BeginTransaction();

            try
            {
                _context.Database.CommitTransaction();
                _context.Add(bill);
                _context.SaveChanges();

                //Data bill details save in DB
                var billDetails = new List<InvoiceDetail>();
                foreach (var item in cartItems)
                {
                    billDetails.Add(new InvoiceDetail
                    {
                        IdInvoice = bill.IdInvoice,
                        IdProduct = item.IdProduct,
                        IdCustomer = user.IdCustomer,
                        UnitPrice = item.Price,
                        Quantity = item.Quantity,
                        TotalPrice = (decimal)item.TotalPrice
                    });
                }

                _context.AddRange(billDetails);
                _context.SaveChanges();

                result = Constants.SUCCESS;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return result;
        }

        public List<CartItem> GetCartItems()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Decrease quantity item in cart
        /// </summary>
        /// <param name="cartItems"></param>
        /// <param name="idProduct"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public List<CartItem> ReduceQuantityProduct(List<CartItem> cartItems, int idProduct, int quantity)
        {
            //Find item exsist cart?
            var item = cartItems.Find(x => x.IdProduct == idProduct);

            //Check item reduce cart
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

        /// <summary>
        /// Delete item ready in cart
        /// </summary>
        /// <param name="cartItems"></param>
        /// <param name="idProduct"></param>
        /// <returns></returns>
        public List<CartItem> RevomeProductToCart(List<CartItem> cartItems, int idProduct)
        {
            //Find item exsist cart?
            var item = cartItems.Find(x => x.IdProduct == idProduct);

            //Check item add cart
            if (item != null)
            {
                //Revome cart
                cartItems.Remove(item);
            }

            return cartItems;
        }
    }
}
