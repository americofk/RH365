using D365_API_Nomina.Core.Application.CommandsAndQueries.Employees;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.EmployeePositions;
using D365_API_Nomina.Core.Application.Common.Model.Employees;
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
    [Route("api/v2.0/employees")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class EmployeeController : ControllerBase
    {
        private readonly IQueryHandler<Employee> _QueryHandler;
        private readonly IEmployeeCommandHandler _CommandHandler;

        public EmployeeController(IQueryHandler<Employee> queryHandler, IEmployeeCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;
        }

        [HttpGet("enabled")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeEnabled, View = true)]
        public async Task<ActionResult> GetEnabled([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, new string[]{ "true", WorkStatus.Employ.ToString()});
            return StatusCode(objectresult.StatusHttp, objectresult);

        }

        [HttpGet("enabled/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeEnabled, View = true)]
        public async Task<ActionResult> GetIdEnabled(string id)
        {
            var objectresult = await _QueryHandler.GetId(id);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        //[HttpPost("enabled")]
        //[AuthorizePrivilege(MenuId = MenuConst.EmployeeEnabled, Edit = true)]
        //public async Task<ActionResult> Post([FromBody] EmployeeRequest model)
        //{
        //    var objectresult = await _CommandHandler.Create(model);
        //    return StatusCode(objectresult.StatusHttp, objectresult);
        //}


        //[HttpDelete("enabled")]
        //[AuthorizePrivilege(MenuId = MenuConst.EmployeeEnabled, Delete = true)]
        //public async Task<ActionResult> DeleteEnabled([FromBody] List<string> ids)
        //{
        //    var objectresult = await _CommandHandler.Delete(ids);
        //    return StatusCode(objectresult.StatusHttp, objectresult);
        //}

        [HttpPut("enabled/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeEnabled, Edit = true)]
        public async Task<ActionResult> Update([FromBody] EmployeeRequest model, string id)
        {
            var objectresult = await _CommandHandler.Update(id, model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPut("enabled/updatestatus/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeEnabled, Edit = true)]
        public async Task<ActionResult> UpdateStatusEnabled(string id)
        {
            var objectresult = await _CommandHandler.UpdateStatus(id, false, false);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }






        [HttpGet("disabled")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeDisabled, View = true)]
        public async Task<ActionResult> GetDisabled([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, new string[] { "false", WorkStatus.Employ.ToString() });
            return StatusCode(objectresult.StatusHttp, objectresult);

        }

        //[HttpDelete("disabled")]
        //[AuthorizePrivilege(MenuId = MenuConst.EmployeeDisabled, Delete = true)]
        //public async Task<ActionResult> DeleteDisabled([FromBody] List<string> ids)
        //{
        //    var objectresult = await _CommandHandler.Delete(ids);
        //    return StatusCode(objectresult.StatusHttp, objectresult);
        //}

        [HttpPut("disabled/updatestatus/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeDisabled, Edit = true)]
        public async Task<ActionResult> UpdateStatusDisabled(string id, [FromQuery] bool isforDgt)
        {
            var objectresult = await _CommandHandler.UpdateStatus(id, true, isforDgt);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }




        [HttpPost("uploadimage/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeImage, Edit = true)]
        public async Task<ActionResult> PostImage([FromForm] EmployeeImageRequest request, string id)
        {
            var objectresult = await _CommandHandler.UploadImage(request, id);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpGet("downloadimage/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeImage, Edit = true)]
        public async Task<ActionResult> GetImage(string id)
        {
            var objectresult = await _CommandHandler.DownloadImage(id);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        //Sección para contratar y despedir empleados

        [HttpGet("candidate")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeCandidate, View = true)]
        public async Task<ActionResult> GetCandidate([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, new string[] { "true", WorkStatus.Candidate.ToString() });
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpGet("candidate/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeCandidate, View = true)]
        public async Task<ActionResult> GetIdCandidate(string id)
        {
            var objectresult = await _QueryHandler.GetId(id);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost("candidate")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeCandidate, Edit = true)]
        public async Task<ActionResult> PostCandidate([FromBody] EmployeeRequest model)
        {
            var objectresult = await _CommandHandler.Create(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPut("candidate/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeCandidate, Edit = true)]
        public async Task<ActionResult> UpdateCandidate([FromBody] EmployeeRequest model, string id)
        {
            var objectresult = await _CommandHandler.Update(id, model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpDelete("candidate")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeCandidate, Delete = true)]
        public async Task<ActionResult> DeleteCandidate([FromBody] List<string> ids)
        {
            var objectresult = await _CommandHandler.Delete(ids);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }



        [HttpGet("dissmis")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeDissmis, View = true)]
        public async Task<ActionResult> GetDismiss([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, new string[] { "true", WorkStatus.Dismissed.ToString() });
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpGet("dissmis/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeDissmis, View = true)]
        public async Task<ActionResult> GetIdDissmis(string id)
        {
            var objectresult = await _QueryHandler.GetId(id);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }



        //Dar de baja al empleado
        [HttpPost("{employeeid}/dissmis")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeEnabled, Edit = true)]
        public async Task<ActionResult> DismissEmployee(EmployeeRequestDismiss model)
        {
            var objectresult = await _CommandHandler.DismissEmployee(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        //Contratar empleado
        [HttpPost("{employeeid}/employ")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeEnabled, Edit = true)]
        public async Task<ActionResult> EmployEmployee(EmployeePositionRequest model)
        {
            var objectresult = await _CommandHandler.AddEmployeetoJob(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }

}
