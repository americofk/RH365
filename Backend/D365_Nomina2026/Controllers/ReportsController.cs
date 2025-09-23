using D365_API_Nomina.Core.Application.CommandsAndQueries.Reports;
using D365_API_Nomina.Core.Domain.Consts;
using D365_API_Nomina.Core.Domain.Enums;
using D365_API_Nomina.WebUI.Attributes;
using D365_API_Nomina.WebUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace D365_API_Nomina.WebUI.Controllers.v2
{
    [Route("api/v2.0/reports")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class ReportsController : ControllerBase
    {
        private readonly IReportQueryHandler _QueryHandler;

        public ReportsController(IReportQueryHandler queryHandler)
        {
            _QueryHandler = queryHandler;
        }

        [HttpGet("payrollpayment")]
        [AuthorizePrivilege(MenuId = MenuConst.PayrollPaymentReport, View = true)]
        public async Task<ActionResult> Get([FromQuery] string payrollprocessid, [FromQuery] string employeeid, [FromQuery] string departmentid)
        {
            var objectresult = await _QueryHandler.PayrollPaymentReport(payrollprocessid, employeeid, departmentid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpGet("payrollresume")]
        [AuthorizePrivilege(MenuId = MenuConst.PayrollProcessReport, View = true)]
        public async Task<ActionResult> GetResume([FromQuery] string payrollprocessid, [FromQuery] string departmentid)
        {
            var objectresult = await _QueryHandler.ResumePaymentReport(payrollprocessid, departmentid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        
        
        [HttpGet("payrollprocess")]
        [AuthorizePrivilege(MenuId = MenuConst.PayrollProcessReport, View = true)]
        public async Task<ActionResult> GetPayrollProcess([FromQuery] string payrollprocessid, [FromQuery] string departmentId)
        {
            var objectresult = await _QueryHandler.PayrollProcessReport(payrollprocessid, departmentId);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        
        [HttpGet("employees")]
        //Cambiar la constante del menú para los roles
        [AuthorizePrivilege(MenuId = MenuConst.PayrollProcessReport, View = true)]
        public async Task<ActionResult> GetEmployees([FromQuery] string departmentId)
        {
            var objectresult = await _QueryHandler.EmployeeReport(departmentId);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        
        [HttpGet("tss")]
        //Cambiar la constante del menú para los roles
        [AuthorizePrivilege(MenuId = MenuConst.PayrollProcessReport, View = true)]
        public async Task<ActionResult> GetTss([FromQuery] int year, [FromQuery] int month, [FromQuery] string payrollid, [FromQuery] string typetss)
        {
            var objectresult = await _QueryHandler.TSSReport(year, month, payrollid, typetss);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        
        [HttpGet("dgt4")]
        [AuthorizePrivilege(MenuId = MenuConst.DGT4Report, View = true)]
        public async Task<ActionResult> GetDGT4Report([FromQuery] int year, [FromQuery] int month)
        {
            var objectresult = await _QueryHandler.DGT4Report(year, month);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        
        [HttpGet("dgt2")]
        [AuthorizePrivilege(MenuId = MenuConst.DGT2Report, View = true)]
        public async Task<ActionResult> GetDGT2Report([FromQuery] int year, [FromQuery] int month)
        {
            var objectresult = await _QueryHandler.DGT2Report(year, month);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        
        [HttpGet("dgt3")]
        [AuthorizePrivilege(MenuId = MenuConst.DGT3Report, View = true)]
        public async Task<ActionResult> GetDGT3Report([FromQuery] int year, [FromQuery] int month)
        {
            var objectresult = await _QueryHandler.DGT3Report(year, month);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        
        [HttpGet("dgt5")]
        [AuthorizePrivilege(MenuId = MenuConst.DGT5Report, View = true)]
        public async Task<ActionResult> GetDGT5Report([FromQuery] int year, [FromQuery] int month)
        {
            var objectresult = await _QueryHandler.DGT5Report(year, month);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        
        [HttpGet("dgt9")]
        [AuthorizePrivilege(MenuId = MenuConst.DGT9Report, View = true)]
        public async Task<ActionResult> GetDGT9Report([FromQuery] int year, [FromQuery] int month)
        {
            var objectresult = await _QueryHandler.DGT9Report(year, month);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        
        [HttpGet("dgt12")]
        [AuthorizePrivilege(MenuId = MenuConst.DGT12Report, View = true)]
        public async Task<ActionResult> GetDGT12Report([FromQuery] int year, [FromQuery] int month)
        {
            var objectresult = await _QueryHandler.DGT12Report(year, month);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        //Envío de correo masivo
        [HttpGet("payrollpayment/sendemail")]
        [AuthorizePrivilege(MenuId = MenuConst.PayrollPaymentReport, View = true)]
        public async Task<ActionResult> SendEmail([FromQuery] string payrollprocessid, [FromQuery] string employeeid, [FromQuery] string departmentid)
        {
            var objectresult = await _QueryHandler.PayrollPaymentReport(payrollprocessid, employeeid, departmentid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
