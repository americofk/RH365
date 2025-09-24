// ============================================================================
// Archivo: IDateTime.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Common/Interfaces/IDateTime.cs
// Descripción: Abstracción para operaciones de fecha/hora.
//   - Permite mockear tiempo en pruebas unitarias
//   - Centraliza manejo de zonas horarias
//   - Facilita pruebas determinísticas
// ============================================================================

using System;

namespace RH365.Core.Application.Common.Interfaces
{
    /// <summary>
    /// Servicio para manejo centralizado de fechas y horas.
    /// Permite control en pruebas y manejo consistente de zonas horarias.
    /// </summary>
    public interface IDateTime
    {
        /// <summary>
        /// Obtiene la fecha y hora actual en UTC.
        /// Usado para campos CreatedOn y ModifiedOn.
        /// ISO 27001: Timestamp consistente en UTC.
        /// </summary>
        DateTime UtcNow { get; }

        /// <summary>
        /// Obtiene la fecha y hora actual en zona horaria local (RD).
        /// Santo Domingo, República Dominicana (UTC-4).
        /// </summary>
        DateTime LocalNow { get; }

        /// <summary>
        /// Obtiene solo la fecha actual sin hora (UTC).
        /// Útil para comparaciones de fecha.
        /// </summary>
        DateTime Today { get; }

        /// <summary>
        /// Convierte UTC a hora local de RD.
        /// </summary>
        /// <param name="utcDateTime">Fecha/hora en UTC.</param>
        /// <returns>Fecha/hora en zona horaria de RD.</returns>
        DateTime ToLocalTime(DateTime utcDateTime);

        /// <summary>
        /// Convierte hora local de RD a UTC.
        /// </summary>
        /// <param name="localDateTime">Fecha/hora local.</param>
        /// <returns>Fecha/hora en UTC.</returns>
        DateTime ToUtcTime(DateTime localDateTime);

        /// <summary>
        /// Obtiene el inicio del día en UTC.
        /// </summary>
        /// <param name="date">Fecha de referencia.</param>
        /// <returns>Fecha a las 00:00:00 UTC.</returns>
        DateTime GetStartOfDay(DateTime date);

        /// <summary>
        /// Obtiene el fin del día en UTC.
        /// </summary>
        /// <param name="date">Fecha de referencia.</param>
        /// <returns>Fecha a las 23:59:59.999 UTC.</returns>
        DateTime GetEndOfDay(DateTime date);

        /// <summary>
        /// Obtiene el primer día del mes.
        /// </summary>
        /// <param name="date">Fecha de referencia.</param>
        /// <returns>Primer día del mes.</returns>
        DateTime GetFirstDayOfMonth(DateTime date);

        /// <summary>
        /// Obtiene el último día del mes.
        /// </summary>
        /// <param name="date">Fecha de referencia.</param>
        /// <returns>Último día del mes.</returns>
        DateTime GetLastDayOfMonth(DateTime date);
    }
}