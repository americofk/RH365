// ============================================================================
// Archivo: EarningCodeDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EarningCode/EarningCodeDto.cs
// Descripción:
//   - DTO de lectura para la entidad EarningCode (dbo.EarningCodes)
//   - Incluye claves, datos de negocio, FKs y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EarningCode
{
    /// <summary>
    /// DTO de salida para EarningCode.
    /// </summary>
    public class EarningCodeDto
    {
        // Identificadores
        public long RecID { get; set; }
        public string? ID { get; set; }

        // Datos de negocio
        public string Name { get; set; } = null!;
        public bool IsTSS { get; set; }
        public bool IsISR { get; set; }
        public long? ProjectRefRecID { get; set; }
        public long? ProjCategoryRefRecID { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string? Description { get; set; }
        public int IndexBase { get; set; }
        public decimal MultiplyAmount { get; set; }
        public string? LedgerAccount { get; set; }
        public long? DepartmentRefRecID { get; set; }
        public bool EarningCodeStatus { get; set; }
        public bool IsExtraHours { get; set; }
        public bool IsRoyaltyPayroll { get; set; }
        public bool IsUseDGT { get; set; }
        public bool IsHoliday { get; set; }
        public TimeOnly WorkFrom { get; set; }
        public TimeOnly WorkTo { get; set; }
        public string? Observations { get; set; }

        // Auditoría ISO 27001
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}