using D365_API_Nomina.Core.Application.CommandsAndQueries.Users;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.Users;
using D365_API_Nomina.Core.Domain.Enums;
using D365_API_Nomina.WebUI.Attributes;
using D365_API_Nomina.WebUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D365_API_Nomina.WebUI.Controllers.v2
{
    [Route("api/v2.0/users/options")]
    [Authorize]
    [ApiController]
    //[AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class UserCompanyController : ControllerBase
    {
        private readonly IUserCommandHandler _CommandHandler;

        public UserCompanyController(IUserCommandHandler commandHandler)
        {
            _CommandHandler = commandHandler;
        }

        [HttpPost("changecompany/{companyid}")]
        public async Task<ActionResult> GetNewCompany(string companyid)
        {
            var objectresult = await _CommandHandler.ChangeCompanyUsed(companyid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
