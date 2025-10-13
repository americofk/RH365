using Microsoft.AspNetCore.Mvc;

namespace Dynacorp_Cargo_2025.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
