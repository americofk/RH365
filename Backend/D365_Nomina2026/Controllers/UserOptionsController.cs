using D365_API_Nomina.Core.Application.CommandsAndQueries.Users;
using D365_API_Nomina.Core.Application.Common.Filter;
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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;


namespace D365_API_Nomina.WebUI.Controllers.v2
{
    [Route("api/v2.0/users/options")]
    [Authorize]
    [ApiController]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class UserOptionsController : ControllerBase
    {
        private readonly IQueryHandler<UserResponse> _QueryHandler;
        private readonly IUserCommandHandler _CommandHandler;

        public UserOptionsController(IQueryHandler<UserResponse> queryHandler, IUserCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] UserOptionsRequestUpdate model)
        {
            var objectresult = await _CommandHandler.UpdateOptions(id, model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost("uploadimageuser/{alias}")]
        public async Task<ActionResult> PostImage([FromForm] UserImageRequest request, string alias)
        {
            var objectresult = await _CommandHandler.UploadUserImage(request, alias);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpGet("downloadimageuser/{alias}")]
        public async Task<ActionResult> GetImage(string alias)
        {
            var objectresult = await _CommandHandler.DownloadUserImage(alias);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        
        //[HttpPost("changecompany/{companyid}")]
        //public async Task<ActionResult> GetNewCompany(string companyid)
        //{
        //    var objectresult = await _CommandHandler.ChangeCompanyUsed(companyid);
        //    return StatusCode(objectresult.StatusHttp, objectresult);
        //}
    }
}
