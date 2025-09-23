using D365_API_Nomina.Core.Application.CommandsAndQueries.ReportsTXT;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Domain.Enums;
using D365_API_Nomina.WebUI.Attributes;
using D365_API_Nomina.WebUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.WebUI.Controllers.v2
{
    [Route("api/v2.0/reportstxt")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class ReportTxtController : ControllerBase
    {
        private readonly IDGTTxtQueryHandler _QueryHandler;

        public ReportTxtController(IDGTTxtQueryHandler queryHandler)
        {
            _QueryHandler = queryHandler;
        }

        [HttpGet("dgt3")]
        public async Task<IActionResult> GetDGT3([FromQuery] MonthYearParametersRequest model)
        {
            var objectresult = await _QueryHandler.CreateDGT3(model.Year, model.Month);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        
        [HttpGet("dgt4")]
        public async Task<IActionResult> GetDGT4([FromQuery] MonthYearParametersRequest model)
        {
            var objectresult = await _QueryHandler.CreateDGT4(model.Year, model.Month);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        
        [HttpGet("dgt5")]
        public async Task<IActionResult> GetDGT5([FromQuery] MonthYearParametersRequest model)
        {
            var objectresult = await _QueryHandler.CreateDGT5(model.Year, model.Month);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        
        [HttpGet("dgt2")]
        public async Task<IActionResult> GetDGT2([FromQuery] MonthYearParametersRequest model)
        {
            var objectresult = await _QueryHandler.CreateDGT2(model.Year, model.Month);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        
        [HttpGet("tss")]
        public async Task<IActionResult> GetTSS([FromQuery] MonthYearParametersRequest model, [FromQuery] string payrollid, [FromQuery] string typetss)
        {
            var objectresult = await _QueryHandler.CreateTSS(model.Year, model.Month, payrollid, typetss);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        
        [HttpGet("txtpayroll")]
        public async Task<IActionResult> GetTxtPayroll([FromQuery] string payrollprocessid, [FromQuery] string payrollid)
        {
            var objectresult = await _QueryHandler.CreatePayroll(payrollprocessid, payrollid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
