// ============================================================================
// Archivo: EmployeeDocumentController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Controllers/EmployeeDocumentController.cs
// Descripcion:
//   - Controller para gestion de Documentos de Empleados
//   - Maneja vistas de consulta y reportes
//   - Integrado con sistema de autenticacion
//   - Los documentos se gestionan desde el formulario de empleados
// Estandar: ISO 27001 - Control de acceso a documentos de identidad
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Infrastructure.Services;

namespace RH365.Controllers
{
    public class EmployeeDocumentController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<EmployeeDocumentController> _logger;

        public EmployeeDocumentController(
            IUserContext userContext,
            ILogger<EmployeeDocumentController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        /// <summary>
        /// Vista de lista de todos los documentos del sistema
        /// </summary>
        [HttpGet]
        public IActionResult LP_EmployeeDocuments()
        {
            // Validar autenticacion
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Intento de acceso sin token de autenticacion");
                return RedirectToAction("Login", "Login");
            }

            // Pasar datos de contexto a la vista
            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";

            _logger.LogInformation($"Usuario {_userContext.UserRefRecID} accediendo a lista de documentos");

            return View("~/Views/EmployeeDocument/LP_EmployeeDocuments.cshtml");
        }

        /// <summary>
        /// Vista de documentos vencidos o proximos a vencer
        /// </summary>
        [HttpGet]
        public IActionResult ExpiredDocuments()
        {
            // Validar autenticacion
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Intento de acceso sin token de autenticacion");
                return RedirectToAction("Login", "Login");
            }

            // Pasar datos de contexto a la vista
            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";

            _logger.LogInformation($"Usuario {_userContext.UserRefRecID} consultando documentos vencidos");

            return View("~/Views/EmployeeDocument/ExpiredDocuments.cshtml");
        }

        /// <summary>
        /// Vista de reporte de documentos por tipo
        /// </summary>
        [HttpGet]
        public IActionResult DocumentTypeReport()
        {
            // Validar autenticacion
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Intento de acceso sin token de autenticacion");
                return RedirectToAction("Login", "Login");
            }

            // Pasar datos de contexto a la vista
            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";

            _logger.LogInformation($"Usuario {_userContext.UserRefRecID} accediendo a reporte de tipos de documento");

            return View("~/Views/EmployeeDocument/DocumentTypeReport.cshtml");
        }

        /// <summary>
        /// Nota: La gestion CRUD de documentos se realiza desde el formulario
        /// de empleados (NewEdit_Employee.cshtml) mediante llamadas AJAX al API.
        /// Este controller proporciona vistas de consulta y reportes adicionales.
        /// </summary>
    }
}
