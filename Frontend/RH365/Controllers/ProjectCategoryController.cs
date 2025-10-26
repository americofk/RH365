// ============================================================================
// Archivo: ProjectCategoryController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Controllers/ProjectCategoryController.cs
// Descripción: Controlador MVC para Categorías de Proyecto
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Infrastructure.Services;

namespace RH365.Controllers
{
    public class ProjectCategoryController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<ProjectCategoryController> _logger;

        public ProjectCategoryController(IUserContext userContext, ILogger<ProjectCategoryController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult LP_ProjectCategories()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";

            return View("~/Views/ProjectCategory/LP_ProjectCategories.cshtml");
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
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";
            ViewBag.RecID = recId ?? 0L;
            ViewBag.IsNew = !recId.HasValue || recId.Value <= 0;

            return View("~/Views/ProjectCategory/NewEdit_ProjectCategory.cshtml");
        }
    }
}
