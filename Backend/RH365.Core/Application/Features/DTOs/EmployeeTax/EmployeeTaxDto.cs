// ============================================================================
// Archivo: EmployeeTaxDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeTax/EmployeeTaxDto.cs
// Descripción:
//   - DTO de lectura para la entidad EmployeeTax
//   - Incluye claves, datos y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeTax
{
    public class EmployeeTaxDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
        public long TaxRefRecID { get; set; }
        public long EmployeeRefRecID { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long PayrollRefRecID { get; set; }
        public string? Observations { get; set; }
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}