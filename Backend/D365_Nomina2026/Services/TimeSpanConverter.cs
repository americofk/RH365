// ============================================================================
// Archivo: TimeSpanConverter.cs
// Proyecto: D365_API_Nomina.WEBUI
// Ruta: D365_API_Nomina.WEBUI/Services/Json/TimeSpanConverter.cs
// Descripción: Conversor System.Text.Json para serializar/deserializar TimeSpan
//              en formato "HH:mm:ss" (o "c" ISO). Evita problemas con JSON por
//              defecto que no maneja TimeSpan.
// ============================================================================

using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace D365_API_Nomina.WEBUI.Services.Json
{
    /// <summary>
    /// Convierte TimeSpan ⇄ string ("HH:mm:ss"). Acepta también formato "c".
    /// </summary>
    public sealed class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        private const string Format = @"hh\:mm\:ss";

        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var s = reader.GetString();
            if (string.IsNullOrWhiteSpace(s))
                return default;

            // Intento 1: "HH:mm:ss"
            if (TimeSpan.TryParseExact(s, Format, CultureInfo.InvariantCulture, out var ts))
                return ts;

            // Intento 2: estándar "c" ([-][d.]hh:mm:ss[.fffffff])
            if (TimeSpan.TryParseExact(s, "c", CultureInfo.InvariantCulture, out ts))
                return ts;

            // Intento 3: parse genérico
            if (TimeSpan.TryParse(s, CultureInfo.InvariantCulture, out ts))
                return ts;

            throw new JsonException($"Valor de TimeSpan inválido: '{s}'. Formato esperado '{Format}' o 'c'.");
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
        }
    }
}
