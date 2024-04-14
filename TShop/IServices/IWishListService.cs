using TShop.ViewModels;

namespace TShop.IServices
{
    public interface IWishListService
    {
        /// <summary>
        /// Handle add list into wishlist
        /// </summary>
        /// <param name="wishListItems"></param>
        /// <param name="idProduct"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        List<WishItem> AddWishListItem(List<WishItem> wishListItems, int idProduct, int quantity);

        /// <summary>
        /// Handle delete the item is ready in wishlist
        /// </summary>
        /// <param name="wishListItems"></param>
        /// <param name="idProduct"></param>
        /// <returns></returns>
        List<WishItem> RevomeWishListItem(List<WishItem> wishListItems, int idProduct);

        /// <summary>
        /// Handle decrease quantity of item ready in wishlist
        /// </summary>
        /// <param name="wishListItems"></param>
        /// <param name="idProduct"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        List<WishItem> ReduceWishListItem(List<WishItem> wishListItems, int idProduct, int quantity);
    }
}
