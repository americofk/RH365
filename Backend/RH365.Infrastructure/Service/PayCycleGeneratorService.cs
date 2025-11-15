// ============================================================================
// Archivo: PayCycleGeneratorService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/PayCycleGeneratorService.cs
// Descripción:
//   - Servicio para generación masiva de ciclos de pago
//   - Calcula fechas según frecuencia de pago (Diary, Weekly, BiWeekly, etc)
//   - Genera N ciclos consecutivos de forma transaccional
//   - Cumple con estándares ISO 27001 para auditoría
// CORRECCIÓN: Lógica incremental de fechas (cada ciclo desde el fin del anterior)
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.PayCycle;
using RH365.Core.Domain.Entities;
using RH365.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RH365.Infrastructure.Services
{
    /// <summary>
    /// Interface del servicio de generación de PayCycles.
    /// </summary>
    public interface IPayCycleGeneratorService
    {
        Task<List<PayCycleDto>> GeneratePayCyclesAsync(
            long payrollRefRecID,
            int quantity,
            CancellationToken ct = default);
    }

    /// <summary>
    /// Servicio para generación automática de múltiples ciclos de pago.
    /// </summary>
    public class PayCycleGeneratorService : IPayCycleGeneratorService
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<PayCycleGeneratorService> _logger;

        public PayCycleGeneratorService(
            IApplicationDbContext context,
            ILogger<PayCycleGeneratorService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Genera múltiples ciclos de pago de forma automática.
        /// </summary>
        /// <param name="payrollRefRecID">RecID del Payroll</param>
        /// <param name="quantity">Cantidad de ciclos a generar</param>
        /// <param name="ct">Token de cancelación</param>
        /// <returns>Lista de PayCycleDtos generados</returns>
        public async Task<List<PayCycleDto>> GeneratePayCyclesAsync(
            long payrollRefRecID,
            int quantity,
            CancellationToken ct = default)
        {
            _logger.LogInformation(
                "Iniciando generación de {Quantity} ciclos para Payroll {PayrollId}",
                quantity,
                payrollRefRecID);

            // 1. Validar que existe el Payroll
            var payroll = await _context.Payrolls
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.RecID == payrollRefRecID, ct);

            if (payroll == null)
            {
                _logger.LogWarning("Payroll {PayrollId} no encontrado", payrollRefRecID);
                throw new InvalidOperationException($"El Payroll con RecID {payrollRefRecID} no existe.");
            }

            // 2. Buscar el último ciclo generado para este Payroll
            var lastCycle = await _context.PayCycles
                .AsNoTracking()
                .Where(pc => pc.PayrollRefRecID == payrollRefRecID)
                .OrderByDescending(pc => pc.PeriodEndDate)
                .ThenByDescending(pc => pc.RecID)
                .FirstOrDefaultAsync(ct);

            // 3. Determinar fecha de inicio
            DateTime startDate;
            if (lastCycle == null)
            {
                // ✅ Primera generación - usar ValidFrom del Payroll
                startDate = payroll.ValidFrom;
                _logger.LogInformation("Primera generación de ciclos. Inicio desde ValidFrom: {StartDate:yyyy-MM-dd}", startDate);
            }
            else
            {
                // ✅ Ya hay ciclos - continuar desde el día siguiente al último
                startDate = lastCycle.PeriodEndDate.AddDays(1);
                _logger.LogInformation(
                    "Continuando desde último ciclo. Fecha final anterior: {LastEnd:yyyy-MM-dd}, Nueva inicio: {StartDate:yyyy-MM-dd}",
                    lastCycle.PeriodEndDate,
                    startDate);
            }

            // 4. Generar los ciclos
            var newCycles = GeneratePayCyclesList(
                payrollRefRecID,
                payroll.PayFrecuency,
                startDate,
                quantity);

            // 5. Guardar en BD usando la estrategia de ejecución para compatibilidad con EnableRetryOnFailure
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync(ct);
                try
                {
                    await _context.PayCycles.AddRangeAsync(newCycles, ct);
                    await _context.SaveChangesAsync(ct);
                    await transaction.CommitAsync(ct);

                    _logger.LogInformation(
                        "Generados exitosamente {Count} ciclos para Payroll {PayrollId}",
                        newCycles.Count,
                        payrollRefRecID);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(ct);
                    _logger.LogError(ex, "Error al guardar ciclos para Payroll {PayrollId}", payrollRefRecID);
                    throw;
                }
            });

            // 6. Mapear a DTOs
            var dtos = newCycles.Select(cycle => new PayCycleDto
            {
                RecID = cycle.RecID,
                ID = cycle.ID,
                PayrollRefRecID = cycle.PayrollRefRecID,
                PeriodStartDate = cycle.PeriodStartDate,
                PeriodEndDate = cycle.PeriodEndDate,
                DefaultPayDate = cycle.DefaultPayDate,
                PayDate = cycle.PayDate,
                AmountPaidPerPeriod = cycle.AmountPaidPerPeriod,
                StatusPeriod = cycle.StatusPeriod,
                IsForTax = cycle.IsForTax,
                IsForTss = cycle.IsForTss,
                Observations = cycle.Observations,
                DataareaID = cycle.DataareaID,
                CreatedBy = cycle.CreatedBy,
                CreatedOn = cycle.CreatedOn,
                ModifiedBy = cycle.ModifiedBy,
                ModifiedOn = cycle.ModifiedOn,
                RowVersion = cycle.RowVersion
            }).ToList();

            return dtos;
        }

        /// <summary>
        /// Genera la lista de ciclos con fechas calculadas según frecuencia.
        /// ✅ CORREGIDO: Cada ciclo se calcula desde el fin del anterior (lógica incremental).
        /// </summary>
        private List<PayCycle> GeneratePayCyclesList(
            long payrollRefRecID,
            int payFrecuency,
            DateTime startDate,
            int quantity)
        {
            var cycles = new List<PayCycle>();
            DateTime currentStartDate = startDate;

            for (int i = 0; i < quantity; i++)
            {
                // ✅ Calcular fecha de fin según frecuencia de pago (desde currentStartDate)
                DateTime periodEndDate = CalculateEndDate(
                    (PayFrecuency)payFrecuency,
                    currentStartDate);

                var cycle = new PayCycle
                {
                    PayrollRefRecID = payrollRefRecID,
                    PeriodStartDate = currentStartDate,
                    PeriodEndDate = periodEndDate,
                    DefaultPayDate = currentStartDate, // Fecha de pago por defecto = inicio del período
                    PayDate = currentStartDate,
                    AmountPaidPerPeriod = 0m,
                    StatusPeriod = 0, // Open
                    IsForTax = false,
                    IsForTss = false,
                    Observations = null
                };

                cycles.Add(cycle);

                // ✅ Preparar la fecha de inicio del siguiente ciclo
                currentStartDate = periodEndDate.AddDays(1);
            }

            return cycles;
        }

        /// <summary>
        /// Calcula la fecha de fin del período según la frecuencia de pago.
        /// ✅ CORREGIDO: Calcula UN solo período desde startDate (no múltiples).
        /// </summary>
        private DateTime CalculateEndDate(PayFrecuency frequency, DateTime startDate)
        {
            DateTime endDate;

            switch (frequency)
            {
                case PayFrecuency.Diary:
                    // Diario: termina el mismo día
                    endDate = startDate;
                    break;

                case PayFrecuency.Weekly:
                    // Semanal: 7 días
                    endDate = startDate.AddDays(6);
                    break;

                case PayFrecuency.TwoWeekly:
                    // Bisemanal: 14 días
                    endDate = startDate.AddDays(13);
                    break;

                case PayFrecuency.BiWeekly:
                    // ✅ Quincenal: 1-15 o 16-fin de mes
                    if (startDate.Day == 1)
                    {
                        // Si inicia el 1, termina el 15
                        endDate = new DateTime(startDate.Year, startDate.Month, 15);
                    }
                    else if (startDate.Day == 16)
                    {
                        // Si inicia el 16, termina el último día del mes
                        endDate = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));
                    }
                    else
                    {
                        // Si inicia en cualquier otro día, ajustar lógica:
                        // - Si es antes del 15, terminar el 15
                        // - Si es después del 15, terminar fin de mes
                        if (startDate.Day < 16)
                        {
                            endDate = new DateTime(startDate.Year, startDate.Month, 15);
                        }
                        else
                        {
                            endDate = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));
                        }
                    }
                    break;

                case PayFrecuency.Monthly:
                    // ✅ Mensual: termina el día anterior al mismo día del mes siguiente
                    // Ejemplo: 15/Jan -> 14/Feb
                    var nextMonth = startDate.AddMonths(1);
                    endDate = nextMonth.AddDays(-1);
                    break;

                case PayFrecuency.ThreeMonth:
                    // Trimestral: 3 meses
                    endDate = startDate.AddMonths(3).AddDays(-1);
                    break;

                case PayFrecuency.FourMonth:
                    // Cuatrimestral: 4 meses
                    endDate = startDate.AddMonths(4).AddDays(-1);
                    break;

                case PayFrecuency.Biannual:
                    // Semestral: 6 meses
                    endDate = startDate.AddMonths(6).AddDays(-1);
                    break;

                case PayFrecuency.Yearly:
                    // Anual: 1 año
                    endDate = startDate.AddYears(1).AddDays(-1);
                    break;

                default:
                    throw new ArgumentException($"Frecuencia de pago no válida: {frequency}");
            }

            return endDate;
        }
    }
}