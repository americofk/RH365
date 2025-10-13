// ============================================================================
// Archivo: CreateEmployeeTaxRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeTax/CreateEmployeeTaxRequest.cs
// Descripción:
//   - DTO de creación para EmployeeTax
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeTax
{
    public class CreateEmployeeTaxRequest
    {
        public long TaxRefRecID { get; set; }
        public long EmployeeRefRecID { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long PayrollRefRecID { get; set; }
        public string? Observations { get; set; }
    }
}