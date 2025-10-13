// ============================================================================
// Archivo: UpdateEmployeeTaxRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeTax/UpdateEmployeeTaxRequest.cs
// Descripción:
//   - DTO de actualización parcial para EmployeeTax
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeTax
{
    public class UpdateEmployeeTaxRequest
    {
        public long? TaxRefRecID { get; set; }
        public long? EmployeeRefRecID { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public long? PayrollRefRecID { get; set; }
        public string? Observations { get; set; }
    }
}