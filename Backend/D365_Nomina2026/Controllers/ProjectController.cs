using D365_API_Nomina.Core.Application.CommandsAndQueries.Projects;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.Projects;
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
    [Route("api/v2.0/projects")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class ProjectController : ControllerBase
    {
        private readonly IQueryHandler<Project> _QueryHandler;
        private readonly IProjectCommandHandler _CommandHandler;

        public ProjectController(IQueryHandler<Project> queryHandler, IProjectCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;
        }

        [HttpGet("enabled")]
        [AuthorizePrivilege(MenuId = MenuConst.ProjectEnabled, View = true)]
        public async Task<ActionResult> GetEnabled([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, true);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpGet("enabled/{projid}")]
        [AuthorizePrivilege(MenuId = MenuConst.ProjectEnabled, View = true)]
        public async Task<ActionResult> GetById(string projid)
        {
            var objectresult = await _QueryHandler.GetId(projid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        //[HttpGet("projects/disabled")]
        //[AuthorizePrivilege(MenuId = MenuConst.ProjectDisabled, View = true)]
        //public async Task<ActionResult> GetDisabled([FromQuery] PaginationFilter paginationFilter)
        //{
        //    var objectresult = await _QueryHandler.GetAll(paginationFilter, false);
        //    return StatusCode(objectresult.StatusHttp, objectresult);

        //}

        [HttpPost("enabled")]
        [AuthorizePrivilege(MenuId = MenuConst.ProjectEnabled, Edit = true)]
        public async Task<ActionResult> Post([FromBody] ProjectRequest model)
        {
            var objectresult = await _CommandHandler.Create(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpDelete("enabled")]
        [AuthorizePrivilege(MenuId = MenuConst.ProjectEnabled, Delete = true)]
        public async Task<ActionResult> Delete([FromBody] List<string> ids)
        {
            var objectresult = await _CommandHandler.Delete(ids);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPut("enabled/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.ProjectEnabled, Edit = true)]
        public async Task<ActionResult> Update([FromBody] ProjectRequestUpdate model, string id)
        {
            var objectresult = await _CommandHandler.Update(id, model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPut("enabled/updatestatus/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.ProjectEnabled, Edit = true)]
        public async Task<ActionResult> UpdateStatus(string id)
        {
            var objectresult = await _CommandHandler.UpdateStatus(id, false);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }




        [HttpGet("disabled")]
        [AuthorizePrivilege(MenuId = MenuConst.ProjectDisabled, View = true)]
        public async Task<ActionResult> GetDisabled([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, false);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpDelete("disabled")]
        [AuthorizePrivilege(MenuId = MenuConst.ProjectDisabled, Delete = true)]
        public async Task<ActionResult> DeleteDisabled([FromBody] List<string> ids)
        {
            var objectresult = await _CommandHandler.Delete(ids);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPut("disabled/updatestatus/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.ProjectDisabled, Edit = true)]
        public async Task<ActionResult> UpdateStatusDisabled(string id)
        {
            var objectresult = await _CommandHandler.UpdateStatus(id, true);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }

}
