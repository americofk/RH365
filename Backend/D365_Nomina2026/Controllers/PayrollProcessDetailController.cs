using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
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
    [Route("api/v2.0/payrollprocessdetails")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class PayrollProcessDetailController : ControllerBase
    {
        private readonly IQueryHandler<PayrollProcessDetail> _QueryHandler;

        public PayrollProcessDetailController(IQueryHandler<PayrollProcessDetail> queryHandler)
        {
            _QueryHandler = queryHandler;
        }

        [HttpGet("{payrollprocessid}")]
        [AuthorizePrivilege(MenuId = MenuConst.PayrollProcess, View = true)]
        public async Task<ActionResult> Get([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter, string payrollprocessid)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, payrollprocessid );
            return StatusCode(objectresult.StatusHttp, objectresult);
        } 
    }
}
