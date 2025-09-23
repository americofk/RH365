using D365_API_Nomina.Core.Domain.Enums;
using D365_API_Nomina.WebUI.Attributes;
using D365_API_Nomina.WebUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace D365_API_Nomina.WebUI.Controllers.v2
{
    [Route("api/v2.0/licensevalidations")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class LicenseValidationController : ControllerBase
    {
        //private readonly ILicenseValidationQueryHandler _QueryHandler;

        //public LicenseValidationController(ILicenseValidationQueryHandler queryHandler)
        //{
        //    _queryHandler = queryHandler;
        //}

        //[HttpGet]
        //public async Task<ActionResult> GetEnabled([FromQuery] string licensekey)
        //{
        //    var objectresult = await _QueryHandler.ValidateLicense(licensekey);
        //    return StatusCode(objectresult.StatusHttp, objectresult);
        //}

        //[HttpGet]
        //public async Task<ActionResult> GetEnabled([FromQuery] string licensekey)
        //{
        //    var objectresult = await _QueryHandler.ValidateLicense(licensekey);
        //    return StatusCode(objectresult.StatusHttp, objectresult);
        //}


    }
}
