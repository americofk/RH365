// ============================================================================
// Archivo: Response.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Common/Models/Response.cs
// Descripción: Modelo genérico de respuesta para operaciones.
//   - Encapsula resultado de operaciones
//   - Maneja errores y mensajes
//   - Usado en todos los handlers
// ============================================================================

using System.Collections.Generic;
using System.Linq;

namespace RH365.Core.Application.Common.Models
{
    /// <summary>
    /// Modelo de respuesta genérico para operaciones del sistema.
    /// Encapsula el resultado, errores y mensajes de cualquier operación.
    /// </summary>
    /// <typeparam name="T">Tipo de dato retornado en la respuesta.</typeparam>
    public class Response<T>
    {
        /// <summary>
        /// Constructor por defecto.
        /// Inicializa una respuesta vacía sin éxito.
        /// </summary>
        public Response()
        {
            Succeeded = false;
            Errors = new List<string>();
        }

        /// <summary>
        /// Constructor con dato de respuesta.
        /// Asume operación exitosa si se proporciona data.
        /// </summary>
        /// <param name="data">Dato a retornar en la respuesta.</param>
        public Response(T data)
        {
            Succeeded = true;
            Data = data;
            Errors = new List<string>();
        }

        /// <summary>
        /// Constructor con estado de éxito explícito.
        /// </summary>
        /// <param name="succeeded">Indica si la operación fue exitosa.</param>
        public Response(bool succeeded)
        {
            Succeeded = succeeded;
            Errors = new List<string>();
        }

        /// <summary>
        /// Indica si la operación fue exitosa.
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Datos retornados por la operación.
        /// Null si hubo error o no hay datos que retornar.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Lista de errores ocurridos durante la operación.
        /// Vacío si la operación fue exitosa.
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Mensaje informativo sobre el resultado.
        /// Puede usarse para éxito o error.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Código HTTP sugerido para la respuesta.
        /// Útil para APIs REST.
        /// </summary>
        public int StatusHttp { get; set; } = 200;

        /// <summary>
        /// Obtiene el primer error de la lista.
        /// Útil para mostrar mensaje principal.
        /// </summary>
        public string? FirstError => Errors?.FirstOrDefault();

        /// <summary>
        /// Verifica si hay errores en la respuesta.
        /// </summary>
        public bool HasErrors => Errors != null && Errors.Any();

        /// <summary>
        /// Crea una respuesta exitosa con datos.
        /// </summary>
        /// <param name="data">Datos a retornar.</param>
        /// <param name="message">Mensaje opcional de éxito.</param>
        /// <returns>Respuesta exitosa configurada.</returns>
        public static Response<T> Success(T data, string? message = null)
        {
            return new Response<T>
            {
                Succeeded = true,
                Data = data,
                Message = message,
                Errors = new List<string>(),
                StatusHttp = 200
            };
        }

        /// <summary>
        /// Crea una respuesta de error con mensaje.
        /// </summary>
        /// <param name="error">Mensaje de error.</param>
        /// <param name="statusHttp">Código HTTP del error.</param>
        /// <returns>Respuesta de error configurada.</returns>
        public static Response<T> Failure(string error, int statusHttp = 400)
        {
            return new Response<T>
            {
                Succeeded = false,
                Errors = new List<string> { error },
                StatusHttp = statusHttp
            };
        }

        /// <summary>
        /// Crea una respuesta de error con múltiples mensajes.
        /// </summary>
        /// <param name="errors">Lista de errores.</param>
        /// <param name="statusHttp">Código HTTP del error.</param>
        /// <returns>Respuesta de error configurada.</returns>
        public static Response<T> Failure(List<string> errors, int statusHttp = 400)
        {
            return new Response<T>
            {
                Succeeded = false,
                Errors = errors,
                StatusHttp = statusHttp
            };
        }

        /// <summary>
        /// Crea una respuesta de "no encontrado".
        /// </summary>
        /// <param name="message">Mensaje descriptivo.</param>
        /// <returns>Respuesta 404 configurada.</returns>
        public static Response<T> NotFound(string message)
        {
            return new Response<T>
            {
                Succeeded = false,
                Errors = new List<string> { message },
                StatusHttp = 404
            };
        }
    }
}