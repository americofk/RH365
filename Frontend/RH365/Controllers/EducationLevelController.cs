// ============================================================================
// Archivo: EducationLevelController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Controllers/EducationLevelController.cs
// Descripción:
//   Controlador MVC para las páginas de Niveles Educativos.
//   - LP_EducationLevels()  → ListPage (LP_EducationLevels.cshtml)
//   - NewEdit()              → Formulario Nuevo/Editar (NewEdit_EducationLevel.cshtml)
// ============================================================================
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Infrastructure.Services;

namespace RH365.Controllers
{
    public class EducationLevelController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<EducationLevelController> _logger;

        public EducationLevelController(IUserContext userContext, ILogger<EducationLevelController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult LP_EducationLevels()
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

            return View("~/Views/EducationLevel/LP_EducationLevels.cshtml");
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

            return View("~/Views/EducationLevel/NewEdit_EducationLevel.cshtml");
        }
    }
}