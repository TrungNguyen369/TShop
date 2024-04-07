using TShop.ViewModels;

namespace TShop.IServices
{
    public interface ICartService
    {
        List<CartItem> GetCartItems();
        List<CartItem> AddProductToCart(List<CartItem> cartItems, int idProduct, int quantity);
        List<CartItem> RevomeproductToCart();
    }
}
