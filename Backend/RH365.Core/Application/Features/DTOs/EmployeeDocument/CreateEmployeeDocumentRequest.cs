// ============================================================================
// Archivo: CreateEmployeeDocumentRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeDocument/CreateEmployeeDocumentRequest.cs
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeDocument
{
    public class CreateEmployeeDocumentRequest
    {
        public long EmployeeRefRecID { get; set; }
        public int DocumentType { get; set; }
        public string DocumentNumber { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public byte[]? FileAttach { get; set; }
        public bool IsPrincipal { get; set; }
        public string? Comment { get; set; }
        public string? Observations { get; set; }
    }
}