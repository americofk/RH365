using D365_API_Nomina.Core.Application.CommandsAndQueries.Reports;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Domain.Consts;
using D365_API_Nomina.Core.Domain.Enums;
using D365_API_Nomina.WebUI.Attributes;
using D365_API_Nomina.WebUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace D365_API_Nomina.WebUI.Controllers.v2
{
    [Route("api/v2.0/emailreports")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class EmailReportsController : ControllerBase
    {
        private readonly IReportCommandHandler _CommandHandler;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ICurrentUserInformation _currentUserInformation;

        public EmailReportsController(IReportCommandHandler commandHandler, IWebHostEnvironment hostingEnvironment, ICurrentUserInformation currentUserInformation)
        {
            _CommandHandler = commandHandler;
            _hostingEnvironment = hostingEnvironment;
            _currentUserInformation = currentUserInformation;
        }

        [HttpPost("payrollpayment")]
        [AuthorizePrivilege(MenuId = MenuConst.PayrollPaymentReport, View = true)]
        public async Task<ActionResult> Post([FromQuery] string payrollprocessid, [FromQuery] string employeeid, [FromQuery] string departmentid)
        {
            var response = _CommandHandler.SendEmail(payrollprocessid, employeeid, departmentid, new string[] { _currentUserInformation.Company, _currentUserInformation.Alias, _currentUserInformation.Email }, _hostingEnvironment.ContentRootPath);

            //return StatusCode(response.StatusHttp, response);

            if (response.IsCompleted)
            {
                var objectresult = await response;
                return StatusCode(objectresult.StatusHttp, objectresult);
            }
            else
            {
                return StatusCode(202, new Response<string>() { Message = $"Se ha iniciado el proceso", StatusHttp = 202 });
            }
        }        
    }
}
