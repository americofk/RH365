using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeePositions;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.EmployeePositions;
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
    [Route("api/v2.0/employeepositions")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class EmployeePositionController : ControllerBase
    {
        private readonly IQueryHandler<EmployeePositionResponse> _QueryHandler;
        private readonly IEmployeePositionCommandHandler _CommandHandler;

        public EmployeePositionController(IQueryHandler<EmployeePositionResponse> queryHandler, IEmployeePositionCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;
        }

        [HttpGet("{employeeid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeePosition, View = true)]
        public async Task<ActionResult> Get([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter, string employeeid)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, employeeid);
            return StatusCode(objectresult.StatusHttp, objectresult);

        }

        [HttpGet("{employeeid}/{positionid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeePosition, View = true)]
        public async Task<ActionResult> GetById(string employeeid, string positionid)
        {
            var objectresult = await _QueryHandler.GetId(new string[] { employeeid, positionid });
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeePosition, Edit = true)]
        public async Task<ActionResult> Post([FromBody] EmployeePositionRequest model)
        {
            var objectresult = await _CommandHandler.Create(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpDelete("{employeeid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeePosition, Delete = true)]
        public async Task<ActionResult> Delete([FromBody] List<string> ids, string employeeid)
        {
            var objectresult = await _CommandHandler.DeleteByParent(ids, employeeid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPut("{employeeid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeePosition, Edit = true)]
        public async Task<ActionResult> Update([FromBody] EmployeePositionRequestUpdate model, string employeeid)
        {
            var objectresult = await _CommandHandler.Update(employeeid, model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPut("updatestatus")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeePosition, Edit = true)]
        public async Task<ActionResult> UpdateStatus(EmployeePositionStatusRequest model)
        {
            var objectresult = await _CommandHandler.UpdateStatus(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
