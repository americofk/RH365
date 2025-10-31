// ============================================================================
// Archivo: CourseInstructorController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Controllers/CourseInstructorController.cs
// Descripción: Controlador MVC para gestión de instructores de curso
// Estándar: ISO 27001 - Control de acceso y autenticación
// ============================================================================
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Infrastructure.Services;

namespace RH365.Controllers
{
    public class CourseInstructorController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<CourseInstructorController> _logger;

        public CourseInstructorController(IUserContext userContext, ILogger<CourseInstructorController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult LP_CourseInstructors()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";

            return View("~/Views/CourseInstructor/LP_CourseInstructors.cshtml");
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

            return View("~/Views/CourseInstructor/NewEdit_CourseInstructor.cshtml");
        }
    }
}
