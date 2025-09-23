using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeLoans;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeLoans;
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
    [Route("api/v2.0/employeeloans")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class EmployeeLoanController : ControllerBase
    {
        private readonly IQueryHandler<EmployeeLoanResponse> _QueryHandler;
        private readonly IEmployeeLoanCommandHandler _CommandHandler;
        private readonly IQueryHandler<EmployeeLoanHistoryResponse> _queryHistoryHandler;

        public EmployeeLoanController(IQueryHandler<EmployeeLoanResponse> queryHandler, IEmployeeLoanCommandHandler commandHandler,
            IQueryHandler<EmployeeLoanHistoryResponse> queryHistoryHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;
            _queryHistoryHandler = queryHistoryHandler;
        }


        [HttpGet("{employeeid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeLoan, View = true)]
        public async Task<ActionResult> GetEnabled([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter ,string employeeid)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter, searchFilter, employeeid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        
        [HttpGet("loanhistories/{employeeid}/{parentinternalid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeLoan, View = true)]
        public async Task<ActionResult> GetHistory([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter ,int parentinternalid, string employeeid)
        {
            var objectresult = await _queryHistoryHandler.GetAll(paginationFilter, searchFilter, new string[] { parentinternalid.ToString(), employeeid });
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpGet("{employeeid}/{internalid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeLoan, View = true)]
        public async Task<ActionResult> GetById(string employeeid, int internalid)
        {
            var objectresult = await _QueryHandler.GetId(new string[] { employeeid, internalid.ToString() });
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPost]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeLoan, Edit = true)]
        public async Task<ActionResult> Post([FromBody] EmployeeLoanRequest model)
        {
            var objectresult = await _CommandHandler.Create(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpDelete("{employeeid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeLoan, Delete = true)]
        public async Task<ActionResult> Delete([FromBody] List<string> ids, string employeeid)
        {
            var objectresult = await _CommandHandler.DeleteByParent(ids, employeeid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPut("{employeeid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeLoan, Edit = true)]
        public async Task<ActionResult> Update([FromBody] EmployeeLoanRequestUpdate model, string employeeid)
        {
            var objectresult = await _CommandHandler.Update(employeeid, model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
