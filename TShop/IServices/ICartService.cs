using TShop.ViewModels;

namespace TShop.IServices
{
    public interface ICartService
    {
        List<CartItem> GetCartItems();
        List<CartItem> AddProductToCart(List<CartItem> cartItems, int idProduct, int quantity);
        List<CartItem> RevomeProductToCart(List<CartItem> cartItems, int idProduct);
        List<CartItem> ReduceQuantityProduct(List<CartItem> cartItems, int idProduct, int quantity);
    }
}
