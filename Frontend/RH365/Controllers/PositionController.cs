// ============================================================================
// Archivo: PositionController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Controllers/PositionController.cs
// Descripción: Controlador MVC para gestión de Posiciones
// Estándar: ISO 27001 - Control A.9.4.1 (Restricción de acceso a la información)
//           Control A.9.2.3 (Gestión de derechos de acceso privilegiados)
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Infrastructure.Services;

namespace RH365.Controllers
{
    public class PositionController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<PositionController> _logger;

        public PositionController(IUserContext userContext, ILogger<PositionController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        /// <summary>
        /// Vista de lista de posiciones (ListPage)
        /// </summary>
        [HttpGet]
        public IActionResult LP_Positions()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";

            return View("~/Views/Position/LP_Positions.cshtml");
        }

        /// <summary>
        /// Vista de formulario para crear/editar posición
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

            return View("~/Views/Position/NewEdit_Position.cshtml");
        }
    }
}
