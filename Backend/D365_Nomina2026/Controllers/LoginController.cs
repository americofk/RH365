// ============================================================================
// Archivo: LoginController.cs
// Proyecto: D365_API_Nomina.WEBUI
// Ruta: D365_API_Nomina.WEBUI/Controllers/v1/LoginController.cs
// Descripción: Controlador API de autenticación (versión v1). Expone el
//              endpoint POST /api/v1/login para emitir el JWT.
// ============================================================================
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using D365_API_Nomina.Core.Application.Contracts.Login;
using D365_API_Nomina.Core.Application.Handlers.Login;

namespace D365_API_Nomina.WEBUI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginCommandHandler _login;

        public LoginController(ILoginCommandHandler login)
        {
            _login = login;
        }

        /// <summary>Autentica y retorna el token + datos básicos del usuario.</summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginRequest model)
        {
            var result = await _login.Login(model);
            if (result is null)
                return Unauthorized(new { Message = "Credenciales inválidas" });

            return Ok(result);
        }
    }
}
