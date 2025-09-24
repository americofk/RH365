// ============================================================================
// Archivo: Result.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Common/Models/Result.cs
// Descripción: Modelo de resultado para operaciones sin retorno de datos.
//   - Usado cuando solo importa éxito/fallo
//   - Más ligero que Response<T>
//   - Ideal para comandos de eliminación o actualización
// ============================================================================

using System.Collections.Generic;
using System.Linq;

namespace RH365.Core.Application.Common.Models
{
    /// <summary>
    /// Resultado de operaciones que no retornan datos.
    /// Más eficiente que Response cuando no se necesita data.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Constructor protegido para forzar uso de métodos factory.
        /// </summary>
        protected Result(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }

        /// <summary>
        /// Indica si la operación fue exitosa.
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Array de errores ocurridos.
        /// Vacío si la operación fue exitosa.
        /// </summary>
        public string[] Errors { get; set; }

        /// <summary>
        /// Mensaje adicional sobre el resultado.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Código de operación para identificar el tipo de resultado.
        /// Útil para manejo específico en cliente.
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Verifica si hay errores.
        /// </summary>
        public bool HasErrors => Errors != null && Errors.Any();

        /// <summary>
        /// Crea un resultado exitoso.
        /// </summary>
        /// <returns>Result configurado como éxito.</returns>
        public static Result Success()
        {
            return new Result(true, new string[] { });
        }

        /// <summary>
        /// Crea un resultado exitoso con mensaje.
        /// </summary>
        /// <param name="message">Mensaje de éxito.</param>
        /// <returns>Result configurado con mensaje.</returns>
        public static Result Success(string message)
        {
            return new Result(true, new string[] { }) { Message = message };
        }

        /// <summary>
        /// Crea un resultado de fallo con errores.
        /// </summary>
        /// <param name="errors">Lista de errores.</param>
        /// <returns>Result configurado como fallo.</returns>
        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors);
        }

        /// <summary>
        /// Crea un resultado de fallo con un error.
        /// </summary>
        /// <param name="error">Mensaje de error.</param>
        /// <returns>Result configurado con error único.</returns>
        public static Result Failure(string error)
        {
            return new Result(false, new[] { error });
        }

        /// <summary>
        /// Convierte implícitamente bool a Result.
        /// True = Success, False = Failure genérico.
        /// </summary>
        /// <param name="succeeded">Estado de éxito.</param>
        public static implicit operator Result(bool succeeded)
        {
            return succeeded ? Success() : Failure("Operación fallida");
        }

        /// <summary>
        /// Convierte implícitamente Result a bool.
        /// Permite usar Result en condicionales directamente.
        /// </summary>
        /// <param name="result">Result a convertir.</param>
        public static implicit operator bool(Result result)
        {
            return result.Succeeded;
        }

        /// <summary>
        /// Combina múltiples Results en uno solo.
        /// Útil para operaciones batch.
        /// </summary>
        /// <param name="results">Results a combinar.</param>
        /// <returns>Result combinado.</returns>
        public static Result Combine(params Result[] results)
        {
            var errors = new List<string>();

            foreach (var result in results)
            {
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors);
                }
            }

            return errors.Any()
                ? Failure(errors)
                : Success();
        }

        /// <summary>
        /// Combina con otro Result.
        /// Si alguno falla, retorna fallo con todos los errores.
        /// </summary>
        /// <param name="other">Otro Result para combinar.</param>
        /// <returns>Result combinado.</returns>
        public Result CombineWith(Result other)
        {
            if (Succeeded && other.Succeeded)
                return Success();

            var allErrors = new List<string>();
            if (!Succeeded) allErrors.AddRange(Errors);
            if (!other.Succeeded) allErrors.AddRange(other.Errors);

            return Failure(allErrors);
        }
    }

    /// <summary>
    /// Result genérico que incluye valor de retorno.
    /// Combina Result con data opcional.
    /// </summary>
    /// <typeparam name="T">Tipo del valor retornado.</typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// Valor retornado si la operación fue exitosa.
        /// </summary>
        public T? Value { get; set; }

        /// <summary>
        /// Constructor protegido.
        /// </summary>
        protected Result(bool succeeded, IEnumerable<string> errors, T? value = default)
            : base(succeeded, errors)
        {
            Value = value;
        }

        /// <summary>
        /// Crea resultado exitoso con valor.
        /// </summary>
        /// <param name="value">Valor a retornar.</param>
        /// <returns>Result con valor.</returns>
        public static Result<T> Success(T value)
        {
            return new Result<T>(true, new string[] { }, value);
        }

        /// <summary>
        /// Crea resultado exitoso con valor y mensaje.
        /// </summary>
        /// <param name="value">Valor a retornar.</param>
        /// <param name="message">Mensaje de éxito.</param>
        /// <returns>Result configurado.</returns>
        public static Result<T> Success(T value, string message)
        {
            return new Result<T>(true, new string[] { }, value) { Message = message };
        }

        /// <summary>
        /// Crea resultado de fallo tipado.
        /// </summary>
        /// <param name="error">Mensaje de error.</param>
        /// <returns>Result de fallo.</returns>
        public static new Result<T> Failure(string error)
        {
            return new Result<T>(false, new[] { error });
        }

        /// <summary>
        /// Crea resultado de fallo con múltiples errores.
        /// </summary>
        /// <param name="errors">Lista de errores.</param>
        /// <returns>Result de fallo.</returns>
        public static new Result<T> Failure(IEnumerable<string> errors)
        {
            return new Result<T>(false, errors);
        }
    }
}