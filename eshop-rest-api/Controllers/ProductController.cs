using Microsoft.AspNetCore.Mvc;

namespace eshop_rest_api.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
