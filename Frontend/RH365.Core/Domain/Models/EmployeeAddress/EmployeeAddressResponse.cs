// ============================================================================
// Archivo: EmployeeAddressResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/EmployeeAddress/EmployeeAddressResponse.cs
// Descripcion: Response para una direccion de empleado individual
// Estandar: ISO 27001 - Respuesta completa con campos de auditoria
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.EmployeeAddress
{
    /// <summary>
    /// Modelo de respuesta para una direccion de empleado
    /// ISO 27001: Incluye todos los campos de auditoria y trazabilidad
    /// </summary>
    public class EmployeeAddressResponse
    {
        /// <summary>
        /// Identificador unico del registro
        /// Campo clave para trazabilidad ISO 27001
        /// </summary>
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        /// <summary>
        /// RecID del empleado asociado
        /// </summary>
        [JsonPropertyName("EmployeeRefRecID")]
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// Nombre completo del empleado (campo calculado)
        /// </summary>
        [JsonPropertyName("EmployeeName")]
        public string EmployeeName { get; set; }

        /// <summary>
        /// Codigo del empleado para facil identificacion
        /// </summary>
        [JsonPropertyName("EmployeeCode")]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// RecID del pais
        /// </summary>
        [JsonPropertyName("CountryRefRecID")]
        public long CountryRefRecID { get; set; }

        /// <summary>
        /// Nombre del pais (campo calculado desde relacion)
        /// </summary>
        [JsonPropertyName("CountryName")]
        public string CountryName { get; set; }

        /// <summary>
        /// Calle de la direccion
        /// </summary>
        [JsonPropertyName("Street")]
        public string Street { get; set; }

        /// <summary>
        /// Numero de casa o apartamento
        /// </summary>
        [JsonPropertyName("Home")]
        public string Home { get; set; }

        /// <summary>
        /// Sector o barrio
        /// </summary>
        [JsonPropertyName("Sector")]
        public string Sector { get; set; }

        /// <summary>
        /// Ciudad
        /// </summary>
        [JsonPropertyName("City")]
        public string City { get; set; }

        /// <summary>
        /// Codigo de provincia
        /// </summary>
        [JsonPropertyName("Province")]
        public string Province { get; set; }

        /// <summary>
        /// Nombre de la provincia (descripcion legible)
        /// </summary>
        [JsonPropertyName("ProvinceName")]
        public string ProvinceName { get; set; }

        /// <summary>
        /// Comentarios adicionales
        /// </summary>
        [JsonPropertyName("Comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Indica si es la direccion principal
        /// </summary>
        [JsonPropertyName("IsPrincipal")]
        public bool IsPrincipal { get; set; }

        // ========================================================================
        // CAMPOS DE AUDITORIA ISO 27001
        // ========================================================================

        /// <summary>
        /// Identificador de la empresa (multiempresa)
        /// ISO 27001: Segregacion de datos por empresa
        /// </summary>
        [JsonPropertyName("DataareaID")]
        public string DataareaID { get; set; }

        /// <summary>
        /// Usuario que creo el registro
        /// ISO 27001: Trazabilidad de creacion
        /// </summary>
        [JsonPropertyName("CreatedBy")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Fecha y hora de creacion del registro
        /// ISO 27001: Timestamp de creacion
        /// </summary>
        [JsonPropertyName("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Usuario que modifico el registro por ultima vez
        /// ISO 27001: Trazabilidad de modificaciones
        /// </summary>
        [JsonPropertyName("ModifiedBy")]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Fecha y hora de ultima modificacion
        /// ISO 27001: Timestamp de ultima modificacion
        /// </summary>
        [JsonPropertyName("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// Version del registro para control de concurrencia
        /// ISO 27001: Control de integridad de datos
        /// </summary>
        [JsonPropertyName("RowVersion")]
        public byte[] RowVersion { get; set; }
    }
}