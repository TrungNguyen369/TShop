using Microsoft.AspNetCore.Mvc;
using TShop.Models;
using TShop.ViewModels;

namespace TShop.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly TshopContext _context;

        public CategoryViewComponent(TshopContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var result = _context.Categories
             .Select(c => new CategoryVM
             {
                 Id = c.IdCategory,
                 Name = c.Name,
                 Quantity = _context.Products.Count(p => p.IdCategory == c.IdCategory)
             })
             .ToList();

            return View("Default", result);
        }
    }
}
