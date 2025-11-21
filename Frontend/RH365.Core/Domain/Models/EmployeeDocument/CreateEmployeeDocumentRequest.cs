// ============================================================================
// Archivo: CreateEmployeeDocumentRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/EmployeeDocument/CreateEmployeeDocumentRequest.cs
// Descripcion: 
//   - Request para crear un documento de empleado
//   - Soporta tipos de documento desde GlobalEnum (Cedula, Pasaporte, etc.)
//   - Validacion de fecha de vencimiento
// Estandar: ISO 27001 - Gestion de documentos de identidad
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.EmployeeDocument
{
    public class CreateEmployeeDocumentRequest
    {
        /// <summary>
        /// Referencia al empleado propietario del documento
        /// </summary>
        [JsonPropertyName("EmployeeRefRecID")]
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// Tipo de documento (0=Cedula, 1=Pasaporte, 2=Residencia, 3=Licencia)
        /// Referencia: GlobalsEnum.DocumentType
        /// </summary>
        [JsonPropertyName("DocumentType")]
        public int DocumentType { get; set; }

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
        /// Comentarios adicionales sobre el documento
        /// </summary>
        [JsonPropertyName("Comment")]
        public string Comment { get; set; }
    }
}
