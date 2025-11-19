// ============================================================================
// Archivo: EmployeeController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Controllers/EmployeeController.cs
// Descripción: Controlador MVC para gestión de empleados
// Estándar: ISO 27001 - Control de acceso y auditoría
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Infrastructure.Services;

namespace RH365.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IUserContext userContext, ILogger<EmployeeController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        /// <summary>
        /// Vista de listado de empleados
        /// </summary>
        [HttpGet]
        public IActionResult LP_Employees()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";

            return View("~/Views/Employee/LP_Employees.cshtml");
        }

        /// <summary>
        /// Vista de creación/edición de empleado
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

            return View("~/Views/Employee/NewEdit_Employee.cshtml");
        }
    }
}