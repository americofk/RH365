using D365_API_Nomina.Core.Application.CommandsAndQueries.Login;
using D365_API_Nomina.Core.Application.Common.Model.User;
using D365_API_Nomina.Core.Application.Common.Model.Users;
using D365_API_Nomina.WebUI.Attributes;
using D365_API_Nomina.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace D365_API_Nomina.WebUI.Controllers.v2
{
    [Route("api/v2.0")]   
    [ApiController]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class LoginController : ControllerBase
    {
        private readonly ILoginCommandHandler _loginCommandHandler;

        public LoginController(ILoginCommandHandler loginCommandHandler)
        {
            _loginCommandHandler = loginCommandHandler;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Post([FromBody] LoginRequest _model)
        {
            var objectresult = await _loginCommandHandler.Login(_model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost]
        [Route("requestchangepassword")]
        public async Task<ActionResult> RequestChangePassword([FromBody] string identification)
        {
            var objectresult = await _loginCommandHandler.RequestChangePassword(identification);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost]
        [Route("sendnewpassword")]
        public async Task<ActionResult> SendNewPassword([FromBody] UserChangePasswordRequest model)
        {
            var objectresult = await _loginCommandHandler.SendNewPassword(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
