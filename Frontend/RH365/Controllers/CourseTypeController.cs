// ============================================================================
// Archivo: CourseTypeController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Controllers/CourseTypeController.cs
// Descripción: Controlador MVC para gestión de tipos de curso
// Estándar: ISO 27001 - Control de acceso y autorización
// ============================================================================
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Infrastructure.Services;

namespace RH365.Controllers
{
    public class CourseTypeController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<CourseTypeController> _logger;

        public CourseTypeController(IUserContext userContext, ILogger<CourseTypeController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult LP_CourseTypes()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            // Usar ViewBag en lugar de modelo anónimo
            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";

            return View("~/Views/CourseType/LP_CourseTypes.cshtml");
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

            return View("~/Views/CourseType/NewEdit_CourseType.cshtml");
        }
    }
}
