using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeDocuments;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeDocuments;
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
    [Route("api/v2.0/employeedocuments")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class EmployeeDocumentController : ControllerBase
    {
        private readonly IQueryHandler<EmployeeDocumentResponse> _QueryHandler;
        private readonly IEmployeeDocumentCommandHandler _CommandHandler;

        public EmployeeDocumentController(IQueryHandler<EmployeeDocumentResponse> queryHandler, IEmployeeDocumentCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;
        }

        [HttpGet("{employeeid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeDocument, View = true)]
        public async Task<ActionResult> Get([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter, string employeeid)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, employeeid);
            return StatusCode(objectresult.StatusHttp, objectresult);

        }

        [HttpGet("{employeeid}/{internalid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeDocument, View = true)]
        public async Task<ActionResult> GetById( string employeeid, int internalid)
        {
            var objectresult = await _QueryHandler.GetId(new string[] { employeeid, internalid.ToString()});
            return StatusCode(objectresult.StatusHttp, objectresult);

        }

        [HttpPost]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeDocument, Edit = true)]
        public async Task<ActionResult> Post([FromBody] EmployeeDocumentRequest model)
        {
            var objectresult = await _CommandHandler.Create(model);
            return StatusCode(objectresult.StatusHttp, objectresult);

        }


        [HttpDelete("{employeeid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeDocument, Delete = true)]
        public async Task<ActionResult> Delete([FromBody] List<string> ids, string employeeid)
        {
            var objectresult = await _CommandHandler.DeleteByEmployee(ids, employeeid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPut("{internalid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeDocument, Edit = true)]
        public async Task<ActionResult> Update([FromBody] EmployeeDocumentRequest model, string internalid)
        {
            var objectresult = await _CommandHandler.Update(internalid, model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost("uploaddocument/{employeeid}/{internalid}")]
        public async Task<ActionResult> PostDocument([FromForm] EmplDocFileRequest request, string employeeid, int internalid)
        {
            var objectresult = await _CommandHandler.UploadDocument(request, employeeid, internalid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpGet("downloaddocument/{employeeid}/{internalid}")]
        public async Task<ActionResult> GetDocument(string employeeid, int internalid)
        {
            var objectresult = await _CommandHandler.DownloadDocument(employeeid, internalid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }

}
