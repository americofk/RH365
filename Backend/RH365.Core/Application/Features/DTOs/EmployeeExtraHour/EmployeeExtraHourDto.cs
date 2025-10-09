// ============================================================================
// Archivo: EmployeeExtraHourDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeExtraHour/EmployeeExtraHourDto.cs
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeExtraHour
{
    public class EmployeeExtraHourDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
        public long EmployeeRefRecID { get; set; }
        public long EarningCodeRefRecID { get; set; }
        public long PayrollRefRecID { get; set; }
        public DateTime WorkedDay { get; set; }
        public TimeOnly StartHour { get; set; }
        public TimeOnly EndHour { get; set; }
        public decimal Amount { get; set; }
        public decimal Indice { get; set; }
        public decimal Quantity { get; set; }
        public int StatusExtraHour { get; set; }
        public DateTime CalcPayrollDate { get; set; }
        public string? Comment { get; set; }
        public string? Observations { get; set; }
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}