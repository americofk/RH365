// ============================================================================
// Archivo: ILoginCommandHandler.cs
// Proyecto: D365_API_Nomina.Core
// Ruta: D365_API_Nomina.Core/Application/Handlers/Login/ILoginCommandHandler.cs
// Descripción: Contrato de la operación de Login. Implementación vive en
//              Infrastructure. El Controller depende de esta interfaz.
// ============================================================================

using System.Threading.Tasks;
using D365_API_Nomina.Core.Application.Contracts.Login;

namespace D365_API_Nomina.Core.Application.Handlers.Login
{
    /// <summary>
    /// Contrato para autenticación de usuarios.
    /// </summary>
    public interface ILoginCommandHandler
    {
        /// <summary>
        /// Autentica o valida existencia según <see cref="LoginRequest.IsValidateUser"/>.
        /// Retorna <see cref="LoginResponse"/> o null si credenciales inválidas.
        /// </summary>
        Task<LoginResponse?> Login(LoginRequest model);
    }
}
