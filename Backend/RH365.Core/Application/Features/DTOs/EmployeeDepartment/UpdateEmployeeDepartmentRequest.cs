// ============================================================================
// Archivo: UpdateEmployeeDepartmentRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeDepartment/UpdateEmployeeDepartmentRequest.cs
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeDepartment
{
    public class UpdateEmployeeDepartmentRequest
    {
        public long? EmployeeRefRecID { get; set; }
        public long? DepartmentRefRecID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? EmployeeDepartmentStatus { get; set; }
        public string? Observations { get; set; }
    }
}