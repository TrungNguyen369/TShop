using Microsoft.AspNetCore.Mvc;
using TShop.Models;
using TShop.ViewModels;

namespace TShop.ViewComponents
{
    public class BrandViewComponent : ViewComponent
    {
        private readonly TshopContext _context;

        public BrandViewComponent(TshopContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var result = _context.Brands
                .Select(x => new BrandVM
                {
                    Id = x.IdBrands,
                    Name = x.Name,
                    Quantity = x.Products.Count(p => p.IdBrands == x.IdBrands)
                }).ToList();
            return View(result);
        }
    }
}
