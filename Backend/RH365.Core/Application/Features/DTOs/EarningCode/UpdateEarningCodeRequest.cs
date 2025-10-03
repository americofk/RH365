// ============================================================================
// Archivo: UpdateEarningCodeRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EarningCode/UpdateEarningCodeRequest.cs
// Descripción:
//   - DTO de actualización parcial para EarningCode (dbo.EarningCodes)
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EarningCode
{
    /// <summary>
    /// Payload para actualizar (parcial) un código de percepción.
    /// </summary>
    public class UpdateEarningCodeRequest
    {
        public string? Name { get; set; }
        public bool? IsTSS { get; set; }
        public bool? IsISR { get; set; }
        public long? ProjectRefRecID { get; set; }
        public long? ProjCategoryRefRecID { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string? Description { get; set; }
        public int? IndexBase { get; set; }
        public decimal? MultiplyAmount { get; set; }
        public string? LedgerAccount { get; set; }
        public long? DepartmentRefRecID { get; set; }
        public bool? EarningCodeStatus { get; set; }
        public bool? IsExtraHours { get; set; }
        public bool? IsRoyaltyPayroll { get; set; }
        public bool? IsUseDGT { get; set; }
        public bool? IsHoliday { get; set; }
        public TimeOnly? WorkFrom { get; set; }
        public TimeOnly? WorkTo { get; set; }
        public string? Observations { get; set; }
    }
}