using D365_API_Nomina.Core.Application.CommandsAndQueries.Batchs;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.Batchs;
using D365_API_Nomina.Core.Domain.Consts;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using D365_API_Nomina.WebUI.Attributes;
using D365_API_Nomina.WebUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace D365_API_Nomina.WebUI.Controllers.v2
{
    [Route("api/v2.0/importbatch")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.LocalAdmin)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class BatchImportDataController : ControllerBase
    {
        private readonly IImportBatchDataCommandHandler _CommandHandler;
        private readonly IQueryAllHandler<BatchHistory> _QueryHandler;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ICurrentUserInformation _currentUserInformation;

        public BatchImportDataController(IImportBatchDataCommandHandler commandHandler, 
            IQueryAllHandler<BatchHistory> queryHandler,
            ICurrentUserInformation currentUserInformation,
            IWebHostEnvironment hostingEnvironment)
        {
            _CommandHandler = commandHandler;
            _QueryHandler = queryHandler;
            _hostingEnvironment = hostingEnvironment;
            _currentUserInformation = currentUserInformation;
        }


        [HttpGet]
        //[AuthorizePrivilege(MenuId = MenuConst.EmployeeEnabled, View = true)]
        public async Task<ActionResult> GetEnabled([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter, searchFilter);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpDelete]
        [AuthorizePrivilege(MenuId = MenuConst.BatchHistory, Delete = true)]
        public async Task<ActionResult> Delete(List<string> ids)
        {
            var objectresult = await _CommandHandler.Delete(ids);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPost("employees")]
        [RequestSizeLimit(30000000)]
        [AuthorizePrivilege(MenuId = MenuConst.BatchHistory, Edit = true)]
        public async Task<ActionResult> PostEmployee([FromBody] List<BatchEmployeeRequest> models)
        {           

            var response = _CommandHandler.Create(models,_hostingEnvironment.ContentRootPath, new string[] { _currentUserInformation.Company, _currentUserInformation.Alias });


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
        
        [HttpPost("employeeaddress")]
        [RequestSizeLimit(30000000)]
        [AuthorizePrivilege(MenuId = MenuConst.BatchHistory, Edit = true)]
        public async Task<ActionResult> PostEmployeeAddress([FromBody] List<BatchAddressEmployeeRequest> models)
        {           

            var response = _CommandHandler.Create(models,_hostingEnvironment.ContentRootPath,new string[] { _currentUserInformation.Company, _currentUserInformation.Alias } );

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


        [HttpPost("employeecontactinfo")]
        [RequestSizeLimit(30000000)]
        [AuthorizePrivilege(MenuId = MenuConst.BatchHistory, Edit = true)]
        public async Task<ActionResult> PostEmployeeContact([FromBody] List<BatchContactInfoEmployeeRequest> models)
        {
            var response = _CommandHandler.Create(models, _hostingEnvironment.ContentRootPath, new string[] { _currentUserInformation.Company, _currentUserInformation.Alias });

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


        [HttpPost("employeebankaccount")]
        [RequestSizeLimit(30000000)]
        [AuthorizePrivilege(MenuId = MenuConst.BatchHistory, Edit = true)]
        public async Task<ActionResult> PostEmployeeBankAccount([FromBody] List<BatchBankAccountEmployeeRequest> models)
        {
            var response = _CommandHandler.Create(models, _hostingEnvironment.ContentRootPath, new string[] { _currentUserInformation.Company, _currentUserInformation.Alias });

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
        
        [HttpPost("employeedocument")]
        [RequestSizeLimit(30000000)]
        [AuthorizePrivilege(MenuId = MenuConst.BatchHistory, Edit = true)]
        public async Task<ActionResult> PostEmployeeDocument([FromBody] List<BatchDocumentEmployeeRequest> models)
        {
            var response = _CommandHandler.Create(models, _hostingEnvironment.ContentRootPath, new string[] { _currentUserInformation.Company, _currentUserInformation.Alias });

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
        
        [HttpPost("employeetax")]
        [RequestSizeLimit(30000000)]
        [AuthorizePrivilege(MenuId = MenuConst.BatchHistory, Edit = true)]
        public async Task<ActionResult> PostEmployeeTax([FromBody] List<BatchTaxEmployeeRequest> models)
        {
            var response = _CommandHandler.Create(models, _hostingEnvironment.ContentRootPath, new string[] { _currentUserInformation.Company, _currentUserInformation.Alias });

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

        [HttpPost("employeeextrahours")]
        [RequestSizeLimit(30000000)]
        [AuthorizePrivilege(MenuId = MenuConst.BatchHistory, Edit = true)]
        public async Task<ActionResult> PostEmployeeExtraHours([FromBody] List<BatchExtraHoursEmployeeRequest> models)
        {
            var response = _CommandHandler.Create(models, _hostingEnvironment.ContentRootPath, new string[] { _currentUserInformation.Company, _currentUserInformation.Alias });

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
        
        
        [HttpPost("employeeearningcodes")]
        [RequestSizeLimit(30000000)]
        [AuthorizePrivilege(MenuId = MenuConst.BatchHistory, Edit = true)]
        public async Task<ActionResult> PostEmployeeEarningCodes([FromBody] List<BatchEarningCodeEmployeeRequest> models)
        {
            var response = _CommandHandler.Create(models, _hostingEnvironment.ContentRootPath, new string[] { _currentUserInformation.Company, _currentUserInformation.Alias });

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
        
        [HttpPost("employeeloans")]
        [RequestSizeLimit(30000000)]
        [AuthorizePrivilege(MenuId = MenuConst.BatchHistory, Edit = true)]
        public async Task<ActionResult> PostEmployeeLoans([FromBody] List<BatchLoanEmployeeRequest> models)
        {
            var response = _CommandHandler.Create(models, _hostingEnvironment.ContentRootPath, new string[] { _currentUserInformation.Company, _currentUserInformation.Alias });

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
        
        [HttpPost("employeedeductions")]
        [RequestSizeLimit(30000000)]
        [AuthorizePrivilege(MenuId = MenuConst.BatchHistory, Edit = true)]
        public async Task<ActionResult> PostEmployeeDeductions([FromBody] List<BatchDeductionCodeEmployeeRequest> models)
        {
            var response = _CommandHandler.Create(models, _hostingEnvironment.ContentRootPath, new string[] { _currentUserInformation.Company, _currentUserInformation.Alias });

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
        
        [HttpPost("courses")]
        [RequestSizeLimit(30000000)]
        [AuthorizePrivilege(MenuId = MenuConst.BatchHistory, Edit = true)]
        public async Task<ActionResult> PostCourse([FromBody] List<BatchDeductionCodeEmployeeRequest> models)
        {
            var response = _CommandHandler.Create(models, _hostingEnvironment.ContentRootPath, new string[] { _currentUserInformation.Company, _currentUserInformation.Alias });

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
        
        [HttpPost("employeeworkcalendars")]
        [RequestSizeLimit(30000000)]
        [AuthorizePrivilege(MenuId = MenuConst.BatchHistory, Edit = true)]
        public async Task<ActionResult> PostEmployeeWorkCalendar([FromBody] List<BatchEmployeeWorkCalendarRequest> models)
        {
            var response = _CommandHandler.Create(models, _hostingEnvironment.ContentRootPath, new string[] { _currentUserInformation.Company, _currentUserInformation.Alias });

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
        
        [HttpPost("employeeworkcontrolcalendars")]
        [RequestSizeLimit(30000000)]
        [AuthorizePrivilege(MenuId = MenuConst.BatchHistory, Edit = true)]
        public async Task<ActionResult> PostEmployeeWorkControlCalendar([FromBody] List<BatchEmployeeWorkControlCalendarRequest> models)
        {
            var response = _CommandHandler.Create(models, _hostingEnvironment.ContentRootPath, new string[] { _currentUserInformation.Company, _currentUserInformation.Alias });

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
