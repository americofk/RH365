// ============================================================================
// Archivo: EmployeeDocumentResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/EmployeeDocument/EmployeeDocumentResponse.cs
// Descripcion: 
//   - Response para un documento de empleado individual
//   - Incluye campos de auditoria ISO 27001
//   - Retorna nombre del tipo de documento y empleado
// Estandar: ISO 27001 - Trazabilidad de documentos de identidad
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.EmployeeDocument
{
    public class EmployeeDocumentResponse
    {
        /// <summary>
        /// Clave primaria del registro
        /// </summary>
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        /// <summary>
        /// Identificador unico del sistema
        /// </summary>
        [JsonPropertyName("ID")]
        public string ID { get; set; }

        /// <summary>
        /// Referencia al empleado propietario del documento
        /// </summary>
        [JsonPropertyName("EmployeeRefRecID")]
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// Nombre completo del empleado (para visualizacion)
        /// </summary>
        [JsonPropertyName("EmployeeName")]
        public string EmployeeName { get; set; }

        /// <summary>
        /// Tipo de documento (0=Cedula, 1=Pasaporte, 2=Residencia, 3=Licencia)
        /// </summary>
        [JsonPropertyName("DocumentType")]
        public int DocumentType { get; set; }

        /// <summary>
        /// Nombre del tipo de documento (para visualizacion)
        /// </summary>
        [JsonPropertyName("DocumentTypeName")]
        public string DocumentTypeName { get; set; }

        /// <summary>
        /// Numero del documento de identidad
        /// </summary>
        [JsonPropertyName("DocumentNumber")]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Fecha de vencimiento del documento
        /// </summary>
        [JsonPropertyName("DueDate")]
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Indica si el documento es el principal
        /// </summary>
        [JsonPropertyName("IsPrincipal")]
        public bool IsPrincipal { get; set; }

        /// <summary>
        /// Indica si el documento tiene archivo adjunto
        /// </summary>
        [JsonPropertyName("HasFileAttach")]
        public bool HasFileAttach { get; set; }

        /// <summary>
        /// Comentarios adicionales sobre el documento
        /// </summary>
        [JsonPropertyName("Comment")]
        public string Comment { get; set; }

        // ========================================================================
        // CAMPOS DE AUDITORIA ISO 27001
        // ========================================================================

        /// <summary>
        /// Empresa/Area de datos
        /// </summary>
        [JsonPropertyName("DataareaID")]
        public string DataareaID { get; set; }

        /// <summary>
        /// Usuario que creo el registro
        /// </summary>
        [JsonPropertyName("CreatedBy")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Fecha de creacion del registro
        /// </summary>
        [JsonPropertyName("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Usuario que modifico el registro por ultima vez
        /// </summary>
        [JsonPropertyName("ModifiedBy")]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Fecha de ultima modificacion del registro
        /// </summary>
        [JsonPropertyName("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// Version de fila para control de concurrencia
        /// </summary>
        [JsonPropertyName("RowVersion")]
        public byte[] RowVersion { get; set; }
    }
}
