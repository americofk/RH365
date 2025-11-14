// ============================================================================
// Archivo: PayCycleGeneratorService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/PayCycleGeneratorService.cs
// Descripción:
//   - Servicio para generación masiva de ciclos de pago
//   - Calcula fechas según frecuencia de pago (Diary, Weekly, BiWeekly, etc)
//   - Genera N ciclos consecutivos de forma transaccional
//   - Cumple con estándares ISO 27001 para auditoría
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
                // Primera generación - usar ValidFrom del Payroll
                startDate = payroll.ValidFrom;
                _logger.LogInformation("Primera generación de ciclos. Inicio: {StartDate}", startDate);
            }
            else
            {
                // Ya hay ciclos - continuar desde el día siguiente al último
                startDate = lastCycle.PeriodEndDate.AddDays(1);
                _logger.LogInformation(
                    "Continuando desde último ciclo. Fecha final anterior: {LastEnd}, Nueva inicio: {StartDate}",
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
        /// </summary>
        private List<PayCycle> GeneratePayCyclesList(
            long payrollRefRecID,
            int payFrecuency,
            DateTime startDate,
            int quantity)
        {
            var cycles = new List<PayCycle>();
            DateTime currentStartDate = startDate;
            DateTime previousEndDate = default;

            for (int i = 1; i <= quantity; i++)
            {
                // Calcular fecha de fin según frecuencia de pago
                DateTime periodEndDate = CalculateEndDate(
                    (PayFrecuency)payFrecuency,
                    currentStartDate,
                    i);

                var cycle = new PayCycle
                {
                    PayrollRefRecID = payrollRefRecID,
                    PeriodStartDate = i == 1 ? currentStartDate : previousEndDate,
                    PeriodEndDate = payFrecuency == (int)PayFrecuency.BiWeekly
                        ? periodEndDate
                        : periodEndDate.AddDays(-1),
                    DefaultPayDate = i == 1 ? currentStartDate : previousEndDate,
                    PayDate = i == 1 ? currentStartDate : previousEndDate,
                    AmountPaidPerPeriod = 0m,
                    StatusPeriod = 0, // Open
                    IsForTax = false,
                    IsForTss = false,
                    Observations = null
                };

                cycles.Add(cycle);

                // Ajustar fechas para el siguiente ciclo
                if (payFrecuency == (int)PayFrecuency.BiWeekly)
                {
                    currentStartDate = periodEndDate.AddDays(1);
                    previousEndDate = currentStartDate;
                }
                else
                {
                    previousEndDate = periodEndDate;
                }
            }

            return cycles;
        }

        /// <summary>
        /// Calcula la fecha de fin del período según la frecuencia de pago.
        /// </summary>
        private DateTime CalculateEndDate(PayFrecuency frequency, DateTime startDate, int cycleNumber)
        {
            DateTime endDate = startDate;

            switch (frequency)
            {
                case PayFrecuency.Diary:
                    endDate = startDate.AddDays(cycleNumber);
                    break;

                case PayFrecuency.Weekly:
                    endDate = startDate.AddDays(7 * cycleNumber);
                    break;

                case PayFrecuency.TwoWeekly:
                    endDate = startDate.AddDays(14 * cycleNumber);
                    break;

                case PayFrecuency.BiWeekly:
                    // Lógica especial: 1-15 o 16-fin de mes (Quincenal)
                    endDate = startDate.Day < 16
                        ? new DateTime(startDate.Year, startDate.Month, 15)
                        : new DateTime(
                            startDate.Month + 1 > 12 ? startDate.Year + 1 : startDate.Year,
                            startDate.Month + 1 > 12 ? 1 : startDate.Month + 1,
                            1).AddDays(-1);
                    break;

                case PayFrecuency.Monthly:
                    endDate = startDate.AddMonths(cycleNumber);
                    break;

                case PayFrecuency.ThreeMonth:
                    endDate = startDate.AddMonths(3 * cycleNumber);
                    break;

                case PayFrecuency.FourMonth:
                    endDate = startDate.AddMonths(4 * cycleNumber);
                    break;

                case PayFrecuency.Biannual:
                    endDate = startDate.AddMonths(6 * cycleNumber);
                    break;

                case PayFrecuency.Yearly:
                    endDate = startDate.AddYears(cycleNumber);
                    break;

                default:
                    throw new ArgumentException($"Frecuencia de pago no válida: {frequency}");
            }

            return endDate;
        }
    }
}