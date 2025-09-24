// ============================================================================
// Archivo: BuildDtoHelper.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Common/Helpers/BuildDtoHelper.cs
// Descripción: Helper para mapeo automático entre entidades y DTOs.
//   - Mapeo por convención de nombres
//   - Manejo de tipos compatibles
//   - Usado para reducir código repetitivo
// ============================================================================

using System;
using System.Linq;
using System.Reflection;

namespace RH365.Core.Application.Common.Helpers
{
    /// <summary>
    /// Helper genérico para construcción y mapeo de DTOs.
    /// Copia propiedades por nombre entre objetos.
    /// </summary>
    /// <typeparam name="T">Tipo destino del mapeo.</typeparam>
    public static class BuildDtoHelper<T> where T : class
    {
        /// <summary>
        /// Construye un DTO copiando propiedades desde origen.
        /// Mapea propiedades con mismo nombre y tipo compatible.
        /// </summary>
        /// <param name="source">Objeto origen de datos.</param>
        /// <param name="destination">Objeto destino (DTO).</param>
        /// <returns>Objeto destino con propiedades copiadas.</returns>
        public static T OnBuild(object source, T destination)
        {
            // Validación de parámetros
            if (source == null)
                throw new ArgumentNullException(nameof(source), "El objeto origen no puede ser nulo");

            if (destination == null)
                throw new ArgumentNullException(nameof(destination), "El objeto destino no puede ser nulo");

            // Obtener tipos y propiedades
            Type sourceType = source.GetType();
            Type destinationType = destination.GetType();

            // Cache de propiedades para mejor rendimiento
            PropertyInfo[] sourceProperties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] destinationProperties = destinationType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Mapear cada propiedad del destino
            foreach (PropertyInfo destProperty in destinationProperties)
            {
                // Verificar si la propiedad puede escribirse
                if (!destProperty.CanWrite)
                    continue;

                // Buscar propiedad correspondiente en origen
                PropertyInfo? sourceProperty = sourceProperties
                    .FirstOrDefault(p => p.Name.Equals(destProperty.Name, StringComparison.OrdinalIgnoreCase)
                                      && p.CanRead);

                if (sourceProperty == null)
                    continue;

                try
                {
                    // Obtener valor del origen
                    object? sourceValue = sourceProperty.GetValue(source);

                    // Si es null, asignar directamente
                    if (sourceValue == null)
                    {
                        destProperty.SetValue(destination, null);
                        continue;
                    }

                    // Verificar compatibilidad de tipos
                    Type sourcePropertyType = Nullable.GetUnderlyingType(sourceProperty.PropertyType)
                                            ?? sourceProperty.PropertyType;
                    Type destPropertyType = Nullable.GetUnderlyingType(destProperty.PropertyType)
                                          ?? destProperty.PropertyType;

                    // Mapeo directo si los tipos son compatibles
                    if (destPropertyType.IsAssignableFrom(sourcePropertyType))
                    {
                        destProperty.SetValue(destination, sourceValue);
                    }
                    // Conversión para tipos básicos
                    else if (IsConvertibleType(sourcePropertyType) && IsConvertibleType(destPropertyType))
                    {
                        object convertedValue = Convert.ChangeType(sourceValue, destPropertyType);
                        destProperty.SetValue(destination, convertedValue);
                    }
                    // Manejo especial para DateTime a string
                    else if (sourcePropertyType == typeof(DateTime) && destPropertyType == typeof(string))
                    {
                        string dateString = ((DateTime)sourceValue).ToString("yyyy-MM-dd HH:mm:ss");
                        destProperty.SetValue(destination, dateString);
                    }
                    // Manejo especial para string a DateTime
                    else if (sourcePropertyType == typeof(string) && destPropertyType == typeof(DateTime))
                    {
                        if (DateTime.TryParse(sourceValue.ToString(), out DateTime dateValue))
                        {
                            destProperty.SetValue(destination, dateValue);
                        }
                    }
                    // Manejo de enums
                    else if (destPropertyType.IsEnum && sourcePropertyType == typeof(string))
                    {
                        if (Enum.TryParse(destPropertyType, sourceValue.ToString(), true, out object? enumValue))
                        {
                            destProperty.SetValue(destination, enumValue);
                        }
                    }
                    else if (sourcePropertyType.IsEnum && destPropertyType == typeof(string))
                    {
                        destProperty.SetValue(destination, sourceValue.ToString());
                    }
                    else if (sourcePropertyType.IsEnum && destPropertyType == typeof(int))
                    {
                        destProperty.SetValue(destination, (int)sourceValue);
                    }
                    else if (sourcePropertyType == typeof(int) && destPropertyType.IsEnum)
                    {
                        destProperty.SetValue(destination, Enum.ToObject(destPropertyType, sourceValue));
                    }
                }
                catch (Exception ex)
                {
                    // Log del error pero continuar con otras propiedades
                    // En producción, usar ILogger aquí
                    Console.WriteLine($"Error mapeando propiedad {destProperty.Name}: {ex.Message}");
                }
            }

            return destination;
        }

        /// <summary>
        /// Crea una nueva instancia del tipo T y mapea propiedades.
        /// </summary>
        /// <param name="source">Objeto origen de datos.</param>
        /// <returns>Nueva instancia de T con propiedades mapeadas.</returns>
        public static T BuildNew(object source)
        {
            // Crear nueva instancia usando constructor sin parámetros
            T destination = Activator.CreateInstance<T>();
            return OnBuild(source, destination);
        }

        /// <summary>
        /// Verifica si un tipo es convertible usando Convert.ChangeType.
        /// </summary>
        /// <param name="type">Tipo a verificar.</param>
        /// <returns>True si es convertible.</returns>
        private static bool IsConvertibleType(Type type)
        {
            return type.IsPrimitive
                || type == typeof(decimal)
                || type == typeof(string)
                || type == typeof(DateTime)
                || type == typeof(DateTimeOffset)
                || type == typeof(TimeSpan)
                || type == typeof(Guid);
        }
    }
}