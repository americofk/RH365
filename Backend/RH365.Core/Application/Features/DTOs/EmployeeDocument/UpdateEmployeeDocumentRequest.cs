// ============================================================================
// Archivo: UpdateEmployeeDocumentRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeDocument/UpdateEmployeeDocumentRequest.cs
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeDocument
{
    public class UpdateEmployeeDocumentRequest
    {
        public long? EmployeeRefRecID { get; set; }
        public int? DocumentType { get; set; }
        public string? DocumentNumber { get; set; }
        public DateTime? DueDate { get; set; }
        public byte[]? FileAttach { get; set; }
        public bool? IsPrincipal { get; set; }
        public string? Comment { get; set; }
        public string? Observations { get; set; }
    }
}