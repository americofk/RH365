// ============================================================================
// Archivo: CourseController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Controllers/CourseController.cs
// Descripción: Controlador MVC para gestión de cursos
// Estándar: ISO 27001 - Control de acceso y autenticación
// ============================================================================
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Infrastructure.Services;

namespace RH365.Controllers
{
    public class CourseController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<CourseController> _logger;

        public CourseController(IUserContext userContext, ILogger<CourseController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult LP_Courses()
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

            return View("~/Views/Course/LP_Courses.cshtml");
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

            return View("~/Views/Course/NewEdit_Course.cshtml");
        }
    }
}
