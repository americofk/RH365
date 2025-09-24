// ============================================================================
// Archivo: DateTimeService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/Common/DateTimeService.cs
// Descripción: Implementación del servicio de fecha/hora.
//   - Manejo centralizado de fechas UTC y locales
//   - Soporte para zona horaria República Dominicana (UTC-4)
//   - Permite testing con fechas determinísticas
// ============================================================================

using RH365.Core.Application.Common.Interfaces;
using System;

namespace RH365.Infrastructure.Services.Common
{
    /// <summary>
    /// Servicio para manejo centralizado de fechas y horas.
    /// Implementa IDateTime para cumplimiento ISO 27001.
    /// </summary>
    public class DateTimeService : IDateTime
    {
        // Zona horaria República Dominicana (UTC-4 todo el año, no cambia por DST)
        private static readonly TimeZoneInfo _dominicanTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time");

        /// <summary>
        /// Obtiene la fecha y hora actual en UTC.
        /// ISO 27001: Timestamp consistente para auditoría.
        /// </summary>
        public DateTime UtcNow => DateTime.UtcNow;

        /// <summary>
        /// Obtiene la fecha y hora actual en zona horaria local (RD).
        /// </summary>
        public DateTime LocalNow => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _dominicanTimeZone);

        /// <summary>
        /// Obtiene solo la fecha actual sin hora (UTC).
        /// </summary>
        public DateTime Today => DateTime.UtcNow.Date;

        /// <summary>
        /// Convierte UTC a hora local de República Dominicana.
        /// </summary>
        /// <param name="utcDateTime">Fecha/hora en UTC.</param>
        /// <returns>Fecha/hora en zona horaria de RD (UTC-4).</returns>
        public DateTime ToLocalTime(DateTime utcDateTime)
        {
            if (utcDateTime.Kind == DateTimeKind.Unspecified)
            {
                // Asumir que es UTC si no está especificado
                utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
            }
            else if (utcDateTime.Kind == DateTimeKind.Local)
            {
                // Convertir a UTC primero
                utcDateTime = utcDateTime.ToUniversalTime();
            }

            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, _dominicanTimeZone);
        }

        /// <summary>
        /// Convierte hora local de RD a UTC.
        /// </summary>
        /// <param name="localDateTime">Fecha/hora local de RD.</param>
        /// <returns>Fecha/hora en UTC.</returns>
        public DateTime ToUtcTime(DateTime localDateTime)
        {
            if (localDateTime.Kind == DateTimeKind.Utc)
            {
                return localDateTime;
            }

            // Especificar como no especificado para la conversión
            if (localDateTime.Kind == DateTimeKind.Local)
            {
                localDateTime = DateTime.SpecifyKind(localDateTime, DateTimeKind.Unspecified);
            }

            return TimeZoneInfo.ConvertTimeToUtc(localDateTime, _dominicanTimeZone);
        }

        /// <summary>
        /// Obtiene el inicio del día en UTC (00:00:00.000).
        /// </summary>
        /// <param name="date">Fecha de referencia.</param>
        /// <returns>Fecha a las 00:00:00 UTC.</returns>
        public DateTime GetStartOfDay(DateTime date)
        {
            return date.Date;
        }

        /// <summary>
        /// Obtiene el fin del día en UTC (23:59:59.999).
        /// </summary>
        /// <param name="date">Fecha de referencia.</param>
        /// <returns>Fecha a las 23:59:59.999 UTC.</returns>
        public DateTime GetEndOfDay(DateTime date)
        {
            return date.Date.AddDays(1).AddMilliseconds(-1);
        }

        /// <summary>
        /// Obtiene el primer día del mes.
        /// </summary>
        /// <param name="date">Fecha de referencia.</param>
        /// <returns>Primer día del mes (día 1).</returns>
        public DateTime GetFirstDayOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0, date.Kind);
        }

        /// <summary>
        /// Obtiene el último día del mes.
        /// </summary>
        /// <param name="date">Fecha de referencia.</param>
        /// <returns>Último día del mes.</returns>
        public DateTime GetLastDayOfMonth(DateTime date)
        {
            var firstDayOfMonth = GetFirstDayOfMonth(date);
            var lastDay = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // Mantener el mismo DateTimeKind
            return new DateTime(lastDay.Year, lastDay.Month, lastDay.Day,
                              23, 59, 59, 999, date.Kind);
        }
    }
}