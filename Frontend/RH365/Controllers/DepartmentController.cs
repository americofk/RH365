// ============================================================================
// Archivo: DepartmentController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Controllers/DepartmentController.cs
// Descripción:
//   - Controlador MVC para vistas de Departamentos
//   - Gestiona ListPage y Formulario Crear/Editar
// Estándar: ISO 27001 - Control de acceso y autorización
// ============================================================================
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Infrastructure.Services;

namespace RH365.Controllers
{
    /// <summary>
    /// Controlador para gestión de vistas de Departamentos
    /// </summary>
    public class DepartmentController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(IUserContext userContext, ILogger<DepartmentController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        /// <summary>
        /// Vista de lista de departamentos con DataTables
        /// </summary>
        [HttpGet]
        public IActionResult LP_Departments()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";

            return View("~/Views/Department/LP_Departments.cshtml");
        }

        /// <summary>
        /// Vista de formulario para crear o editar departamento
        /// </summary>
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

            return View("~/Views/Department/NewEdit_Department.cshtml");
        }
    }
}
