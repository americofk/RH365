// ============================================================================
// Archivo: CreateEarningCodeRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EarningCode/CreateEarningCodeRequest.cs
// Descripción:
//   - DTO de creación para EarningCode (dbo.EarningCodes)
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EarningCode
{
    /// <summary>
    /// Payload para crear un código de percepción.
    /// </summary>
    public class CreateEarningCodeRequest
    {
        /// <summary>Nombre de la percepción.</summary>
        public string Name { get; set; } = null!;

        /// <summary>Afecta TSS.</summary>
        public bool IsTSS { get; set; }

        /// <summary>Afecta ISR.</summary>
        public bool IsISR { get; set; }

        /// <summary>FK al proyecto (opcional).</summary>
        public long? ProjectRefRecID { get; set; }

        /// <summary>FK a la categoría del proyecto (opcional).</summary>
        public long? ProjCategoryRefRecID { get; set; }

        /// <summary>Vigente desde.</summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>Vigente hasta.</summary>
        public DateTime ValidTo { get; set; }

        /// <summary>Descripción (opcional).</summary>
        public string? Description { get; set; }

        /// <summary>Índice base de cálculo.</summary>
        public int IndexBase { get; set; }

        /// <summary>Monto multiplicador.</summary>
        public decimal MultiplyAmount { get; set; }

        /// <summary>Cuenta contable (opcional).</summary>
        public string? LedgerAccount { get; set; }/// <summary>FK al departamento (opcional).</summary>
        public long? DepartmentRefRecID { get; set; }

        /// <summary>Estado activo/inactivo.</summary>
        public bool EarningCodeStatus { get; set; } = true;

        /// <summary>Corresponde a horas extras.</summary>
        public bool IsExtraHours { get; set; }

        /// <summary>Corresponde a regalías (salario 13).</summary>
        public bool IsRoyaltyPayroll { get; set; }

        /// <summary>Se usa para reportes a la DGT.</summary>
        public bool IsUseDGT { get; set; }

        /// <summary>Corresponde a feriados.</summary>
        public bool IsHoliday { get; set; }

        /// <summary>Hora de inicio de jornada.</summary>
        public TimeOnly WorkFrom { get; set; }

        /// <summary>Hora de fin de jornada.</summary>
        public TimeOnly WorkTo { get; set; }

        /// <summary>Observaciones (opcional).</summary>
        public string? Observations { get; set; }
    }
}