using D365_API_Nomina.Core.Application.CommandsAndQueries.Jobs;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.Jobs;
using D365_API_Nomina.Core.Domain.Consts;
using D365_API_Nomina.Core.Domain.Entities;
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
    [Route("api/v2.0/jobs")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class JobController : ControllerBase
    {
        private readonly IQueryHandler<Job> _QueryHandler;
        private readonly IJobCommandHandler _CommandHandler;

        public JobController(IQueryHandler<Job> queryHandler, IJobCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;
        }


        [HttpGet("enabled")]
        [AuthorizePrivilege(MenuId = MenuConst.JobEnabled, View = true)]
        public async Task<ActionResult> GetEnabled([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, true);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpGet("enabled/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.JobEnabled, View = true)]
        public async Task<ActionResult> GetById(string id)
        {
            var objectresult = await _QueryHandler.GetId(new string[] { "true", id });
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpGet("disabled")]
        [AuthorizePrivilege(MenuId = MenuConst.JobDisabled, View = true)]
        public async Task<ActionResult> GetDisabled([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, false);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPost("enabled")]
        [AuthorizePrivilege(MenuId = MenuConst.JobEnabled, Edit = true)]
        public async Task<ActionResult> Post([FromBody] JobRequest model)
        {
            var objectresult = await _CommandHandler.Create(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpDelete("enabled")]
        [AuthorizePrivilege(MenuId = MenuConst.JobEnabled, Delete = true)]
        public async Task<ActionResult> DeleteEnabled([FromBody] List<string> ids)
        {
            var objectresult = await _CommandHandler.Delete(ids);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpDelete("disabled")]
        [AuthorizePrivilege(MenuId = MenuConst.JobDisabled, Delete = true)]
        public async Task<ActionResult> DeleteDisabled([FromBody] List<string> ids)
        {
            var objectresult = await _CommandHandler.Delete(ids);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPut("enabled/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.JobEnabled, Edit = true)]
        public async Task<ActionResult> Update([FromBody] JobRequest model, string id)
        {
            var objectresult = await _CommandHandler.Update(id, model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPut("enabled/updatestatus/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.JobEnabled, Edit = true)]
        public async Task<ActionResult> UpdateStatusEnabled(string id, bool status)
        {
            var objectresult = await _CommandHandler.UpdateStatus(id, status);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPut("disabled/updatestatus/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.JobDisabled, Edit = true)]
        public async Task<ActionResult> UpdateStatusDisabled(string id, bool status)
        {
            var objectresult = await _CommandHandler.UpdateStatus(id, status);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
