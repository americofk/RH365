// ============================================================================
// Archivo: EmployeeContactInfoResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/EmployeeContactInfo/EmployeeContactInfoResponse.cs
// Descripcion: Response para informacion de contacto de empleado individual
// Estandar: ISO 27001 - Incluye campos de auditoria obligatorios
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.EmployeeContactInfo
{
    /// <summary>
    /// Response para informacion de contacto de empleado
    /// Incluye campos de auditoria segun estandar ISO 27001
    /// </summary>
    public class EmployeeContactInfoResponse
    {
        /// <summary>
        /// Identificador unico del registro (Clave Primaria)
        /// </summary>
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        /// <summary>
        /// ID sistema generado automaticamente
        /// </summary>
        [JsonPropertyName("ID")]
        public string ID { get; set; }

        /// <summary>
        /// Referencia al empleado (RecID)
        /// </summary>
        [JsonPropertyName("EmployeeRefRecID")]
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// Nombre completo del empleado (FirstName + LastName)
        /// </summary>
        [JsonPropertyName("EmployeeName")]
        public string EmployeeName { get; set; }

        /// <summary>
        /// Tipo de contacto
        /// 0 = Celular, 1 = Correo, 2 = Telefono, 3 = Otro
        /// </summary>
        [JsonPropertyName("ContactType")]
        public int ContactType { get; set; }

        /// <summary>
        /// Nombre del tipo de contacto (traduccion del enum)
        /// </summary>
        [JsonPropertyName("ContactTypeName")]
        public string ContactTypeName { get; set; }

        /// <summary>
        /// Valor del contacto (numero, email, etc)
        /// </summary>
        [JsonPropertyName("ContactValue")]
        public string ContactValue { get; set; }

        /// <summary>
        /// Indica si es el contacto principal
        /// </summary>
        [JsonPropertyName("IsPrimary")]
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Comentarios adicionales
        /// </summary>
        [JsonPropertyName("Comments")]
        public string Comments { get; set; }

        // ========================================================================
        // CAMPOS DE AUDITORIA ISO 27001
        // ========================================================================

        /// <summary>
        /// Empresa a la que pertenece el registro
        /// </summary>
        [JsonPropertyName("DataareaID")]
        public string DataareaID { get; set; }

        /// <summary>
        /// Usuario que creo el registro
        /// </summary>
        [JsonPropertyName("CreatedBy")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Fecha y hora de creacion del registro
        /// </summary>
        [JsonPropertyName("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Usuario que realizo la ultima modificacion
        /// </summary>
        [JsonPropertyName("ModifiedBy")]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Fecha y hora de ultima modificacion
        /// </summary>
        [JsonPropertyName("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// Version del registro para control de concurrencia
        /// </summary>
        [JsonPropertyName("RowVersion")]
        public byte[] RowVersion { get; set; }
    }
}
