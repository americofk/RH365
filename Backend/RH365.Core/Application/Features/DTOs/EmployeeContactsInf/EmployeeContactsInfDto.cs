// ============================================================================
// Archivo: EmployeeContactsInfDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeContactsInf/EmployeeContactsInfDto.cs
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeContactsInf
{
    public class EmployeeContactsInfDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
        public long EmployeeRefRecID { get; set; }
        public int ContactType { get; set; }
        public string ContactValue { get; set; } = null!;
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