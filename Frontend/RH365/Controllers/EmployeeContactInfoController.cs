// ============================================================================
// Archivo: EmployeeContactInfoController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Controllers/EmployeeContactInfoController.cs
// Descripcion: Controller para vistas de Informacion de Contacto de Empleados
// Estandar: ISO 27001 - Control de acceso mediante autenticacion
// Nota: Este controller se usa si se requiere gestion independiente de contactos
//       Normalmente los contactos se gestionan desde el formulario del empleado
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Infrastructure.Services;

namespace RH365.Controllers
{
    public class EmployeeContactInfoController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<EmployeeContactInfoController> _logger;

        public EmployeeContactInfoController(
            IUserContext userContext,
            ILogger<EmployeeContactInfoController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        /// <summary>
        /// Vista de listado de contactos de empleados
        /// Requiere autenticacion mediante token en sesion
        /// </summary>
        [HttpGet]
        public IActionResult LP_EmployeeContactInfo()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";

            return View("~/Views/EmployeeContactInfo/LP_EmployeeContactInfo.cshtml");
        }

        /// <summary>
        /// Vista de crear/editar contacto de empleado
        /// Requiere autenticacion mediante token en sesion
        /// </summary>
        [HttpGet]
        public IActionResult NewEdit(long? recId = null, long? employeeId = null)
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
            ViewBag.EmployeeRefRecID = employeeId ?? 0L;
            ViewBag.IsNew = !recId.HasValue || recId.Value <= 0;

            return View("~/Views/EmployeeContactInfo/NewEdit_EmployeeContactInfo.cshtml");
        }
    }
}
