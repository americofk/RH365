// ============================================================================
// Archivo: EmployeeDepartmentDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeDepartment/EmployeeDepartmentDto.cs
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeDepartment
{
    public class EmployeeDepartmentDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
        public long EmployeeRefRecID { get; set; }
        public long DepartmentRefRecID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool EmployeeDepartmentStatus { get; set; }
        public string? Observations { get; set; }
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}