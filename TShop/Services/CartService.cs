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
        public List<CartItem> AddProductToCart(List<CartItem> cartItems, int idProduct, int quantity)
        {
            //find item exsist cart?
            var item = cartItems.Find(x => x.IdProduct == idProduct);

            //new item add cart
            //defaut add quantity in item in cart
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

                //add cart
                cartItems.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }

            return cartItems;
        }

        public string CheckOut(CheckOutVM checkOutVM, string? payment, List<CartItem> cartItems)
        {
            var result = Constants.FAIL;

            var user = _userService.GetUserByEmail(checkOutVM.Email);

            if (user == null)
            {
                return result;
            }

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

        public List<CartItem> ReduceQuantityProduct(List<CartItem> cartItems, int idProduct, int quantity)
        {
            //find item exsist cart?
            var item = cartItems.Find(x => x.IdProduct == idProduct);

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
            var item = cartItems.Find(x => x.IdProduct == idProduct);

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
