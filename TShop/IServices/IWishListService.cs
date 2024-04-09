using TShop.ViewModels;

namespace TShop.IServices
{
    public interface IWishListService
    {
        List<WishItem> AddWishListItem(List<WishItem> wishListItems, int idProduct, int quantity);
        List<WishItem> RevomeWishListItem(List<WishItem> wishListItems, int idProduct);
        List<WishItem> ReduceWishListItem(List<WishItem> wishListItems, int idProduct, int quantity);
    }
}
