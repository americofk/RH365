// ============================================================================
// Archivo: EmployeeAddressController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Controllers/EmployeeAddressController.cs
// Descripcion: Controlador MVC para gestion de direcciones de empleados
// ISO 27001: Control de acceso y validacion de sesion
// ============================================================================
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Infrastructure.Services;

namespace RH365.Controllers
{
    /// <summary>
    /// Controlador para gestion de direcciones de empleados
    /// ISO 27001: Validacion de autenticacion en cada accion
    /// </summary>
    public class EmployeeAddressController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<EmployeeAddressController> _logger;

        public EmployeeAddressController(IUserContext userContext, ILogger<EmployeeAddressController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        /// <summary>
        /// Vista de lista de direcciones de empleados
        /// ISO 27001: Valida token de sesion antes de mostrar datos
        /// </summary>
        [HttpGet]
        public IActionResult LP_EmployeeAddress()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Intento de acceso sin token valido a lista de direcciones");
                return RedirectToAction("Login", "Login");
            }

            // Usar ViewBag en lugar de modelo anonimo
            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";

            _logger.LogInformation($"Usuario {_userContext.UserRefRecID} accedio a lista de direcciones");
            return View("~/Views/EmployeeAddress/LP_EmployeeAddress.cshtml");
        }

        /// <summary>
        /// Vista de creacion/edicion de direccion de empleado
        /// ISO 27001: Valida token y registra acceso
        /// </summary>
        [HttpGet]
        public IActionResult NewEdit(long? recId = null)
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Intento de acceso sin token valido a formulario de direcciones");
                return RedirectToAction("Login", "Login");
            }

            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";
            ViewBag.RecID = recId ?? 0L;
            ViewBag.IsNew = !recId.HasValue || recId.Value <= 0;

            var action = ViewBag.IsNew ? "crear" : "editar";
            _logger.LogInformation($"Usuario {_userContext.UserRefRecID} accedio a {action} direccion (RecID: {recId ?? 0})");

            return View("~/Views/EmployeeAddress/NewEdit_EmployeeAddress.cshtml");
        }
    }
}
