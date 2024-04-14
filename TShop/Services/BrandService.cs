using TShop.IServices;
using TShop.Models;

namespace TShop.Services
{
    public class BrandService : IBrandService
    {
        private readonly TshopContext _context;

        public BrandService(TshopContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all brand
        /// </summary>
        /// <returns>list brand</returns>
        public List<Brand> GetAllBrand()
        {
            return _context.Brands.ToList(); ;
        }
    }
}
