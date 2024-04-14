using TShop.ViewModels;

namespace TShop.IServices
{
    public interface ICartService
    {
        /// <summary>
        /// Get data cart item
        /// </summary>
        /// <returns></returns>
        List<CartItem> GetCartItems();

        /// <summary>
        /// Add product into cart
        /// </summary>
        /// <param name="cartItems"></param>
        /// <param name="idProduct"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        List<CartItem> AddProductToCart(List<CartItem> cartItems, int idProduct, int quantity);

        /// <summary>
        /// Delete item is ready in cart
        /// </summary>
        /// <param name="cartItems">list cart</param>
        /// <param name="idProduct">id of product</param>
        /// <returns></returns>
        List<CartItem> RevomeProductToCart(List<CartItem> cartItems, int idProduct);

        /// <summary>
        /// Reduce quantity item is ready in cart
        /// </summary>
        /// <param name="cartItems">list cart</param>
        /// <param name="idProduct">id of product</param>
        /// <param name="quantity">quantity wants to decrease</param>
        /// <returns></returns>
        List<CartItem> ReduceQuantityProduct(List<CartItem> cartItems, int idProduct, int quantity);

        /// <summary>
        /// Handle checkout
        /// </summary>
        /// <param name="checkOutVM">view model checkout</param>
        /// <param name="payment">payment method</param>
        /// <param name="cartItems">list cart for pay</param>
        /// <returns></returns>
        string CheckOut(CheckOutVM checkOutVM, string? payment, List<CartItem> cartItems);
    }
}
