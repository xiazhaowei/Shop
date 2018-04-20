using Microsoft.AspNetCore.Mvc;

namespace Shop.Apis.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}