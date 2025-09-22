using Microsoft.AspNetCore.Mvc;

namespace Dynacorp_Cargo_2025.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
