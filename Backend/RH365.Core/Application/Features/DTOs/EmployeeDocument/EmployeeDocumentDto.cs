// ============================================================================
// Archivo: EmployeeDocumentDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeDocument/EmployeeDocumentDto.cs
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeDocument
{
    public class EmployeeDocumentDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
        public long EmployeeRefRecID { get; set; }
        public int DocumentType { get; set; }
        public string DocumentNumber { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public bool HasFileAttach { get; set; }
        public bool IsPrincipal { get; set; }
        public string? Comment { get; set; }
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}