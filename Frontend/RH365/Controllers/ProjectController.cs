// ============================================================================
// Archivo: ProjectController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Controllers/ProjectController.cs
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Infrastructure.Services;

namespace RH365.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(IUserContext userContext, ILogger<ProjectController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult LP_Projects()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            // Usar ViewBag en lugar de modelo anónimo
            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;

            return View("~/Views/Project/LP_Projects.cshtml");
        }

        [HttpGet]
        public IActionResult NewEdit(long? recId = null)
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.RecID = recId ?? 0L;
            ViewBag.IsNew = !recId.HasValue || recId.Value <= 0;

            return View("~/Views/Project/NewEdit_Project.cshtml");
        }
    }
}