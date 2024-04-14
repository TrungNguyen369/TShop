using TShop.Models;

namespace TShop.IServices
{
    public interface IBrandService
    {
        /// <summary>
        /// Get all list brand
        /// </summary>
        /// <returns>list brand</returns>
        List<Brand> GetAllBrand();
    }
}
