// ============================================================================
// Archivo: UpdateEmployeeDocumentRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/EmployeeDocument/UpdateEmployeeDocumentRequest.cs
// Descripcion: 
//   - Request para actualizar un documento de empleado existente
//   - Permite modificar tipo, numero y fecha de vencimiento
//   - Validacion de integridad de datos
// Estandar: ISO 27001 - Mantenimiento de registros de identificacion
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.EmployeeDocument
{
    public class UpdateEmployeeDocumentRequest
    {
        /// <summary>
        /// Tipo de documento (0=Cedula, 1=Pasaporte, 2=Residencia, 3=Licencia)
        /// Referencia: GlobalsEnum.DocumentType
        /// </summary>
        [JsonPropertyName("DocumentType")]
        public int? DocumentType { get; set; }

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
        public bool? IsPrincipal { get; set; }

        /// <summary>
        /// Comentarios adicionales sobre el documento
        /// </summary>
        [JsonPropertyName("Comment")]
        public string Comment { get; set; }
    }
}
