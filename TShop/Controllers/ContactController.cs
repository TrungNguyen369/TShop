using Microsoft.AspNetCore.Mvc;

namespace TShop.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
