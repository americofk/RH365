// ============================================================================
// Archivo: CalendarHolidayController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Controllers/CalendarHolidayController.cs
// ============================================================================
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Infrastructure.Services;

namespace RH365.Controllers
{
    public class CalendarHolidayController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<CalendarHolidayController> _logger;

        public CalendarHolidayController(IUserContext userContext, ILogger<CalendarHolidayController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult LP_CalendarHolidays()
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

            return View("~/Views/CalendarHoliday/LP_CalendarHolidays.cshtml");
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

            return View("~/Views/CalendarHoliday/NewEdit_CalendarHoliday.cshtml");
        }
    }
}
