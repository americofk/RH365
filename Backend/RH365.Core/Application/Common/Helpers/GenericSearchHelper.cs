// ============================================================================
// Archivo: GenericSearchHelper.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Common/Helpers/GenericSearchHelper.cs
// Descripción: Helper para generación de expresiones lambda de búsqueda.
//   - Construcción dinámica de predicados
//   - Soporta múltiples tipos de comparación
//   - Usado con Entity Framework para queries dinámicos
// ============================================================================

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RH365.Core.Application.Common.Filters;

namespace RH365.Core.Application.Common.Helpers
{
    /// <summary>
    /// Helper para generar expresiones lambda de búsqueda dinámicamente.
    /// Permite crear predicados en tiempo de ejecución para Entity Framework.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad a buscar.</typeparam>
    public static class GenericSearchHelper<T> where T : class
    {
        /// <summary>
        /// Genera una expresión lambda desde un filtro de búsqueda.
        /// </summary>
        /// <param name="filter">Filtro con criterios de búsqueda.</param>
        /// <returns>Expresión lambda para usar en Where().</returns>
        public static Expression<Func<T, bool>> GetLambdaExpression(SearchFilter<T> filter)
        {
            // Validar filtro
            if (filter == null || !filter.IsValid())
            {
                // Retornar expresión que siempre es true
                return x => true;
            }

            // Crear parámetro para la expresión lambda
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");

            try
            {
                // Obtener la propiedad por nombre
                PropertyInfo? property = typeof(T).GetProperty(filter.PropertyName!,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (property == null)
                {
                    // Si no existe la propiedad, retornar true
                    return x => true;
                }

                // Crear expresión de acceso a la propiedad
                MemberExpression propertyExpression = Expression.Property(parameter, property);

                // Obtener tipo de la propiedad
                Type propertyType = Nullable.GetUnderlyingType(property.PropertyType)
                                 ?? property.PropertyType;

                // Generar expresión según el tipo de comparación
                Expression? body = filter.Comparison switch
                {
                    SearchComparison.Equals => CreateEqualsExpression(propertyExpression, filter.PropertyValue!, propertyType),
                    SearchComparison.NotEquals => CreateNotEqualsExpression(propertyExpression, filter.PropertyValue!, propertyType),
                    SearchComparison.Contains => CreateContainsExpression(propertyExpression, filter.PropertyValue!),
                    SearchComparison.StartsWith => CreateStartsWithExpression(propertyExpression, filter.PropertyValue!),
                    SearchComparison.EndsWith => CreateEndsWithExpression(propertyExpression, filter.PropertyValue!),
                    SearchComparison.GreaterThan => CreateComparisonExpression(propertyExpression, filter.PropertyValue!, propertyType, Expression.GreaterThan),
                    SearchComparison.GreaterThanOrEqual => CreateComparisonExpression(propertyExpression, filter.PropertyValue!, propertyType, Expression.GreaterThanOrEqual),
                    SearchComparison.LessThan => CreateComparisonExpression(propertyExpression, filter.PropertyValue!, propertyType, Expression.LessThan),
                    SearchComparison.LessThanOrEqual => CreateComparisonExpression(propertyExpression, filter.PropertyValue!, propertyType, Expression.LessThanOrEqual),
                    SearchComparison.IsNullOrEmpty => CreateIsNullOrEmptyExpression(propertyExpression, propertyType),
                    SearchComparison.IsNotNullOrEmpty => CreateIsNotNullOrEmptyExpression(propertyExpression, propertyType),
                    _ => Expression.Constant(true)
                };

                // Si no se pudo crear la expresión, retornar true
                if (body == null)
                {
                    return x => true;
                }

                // Crear y retornar la expresión lambda completa
                return Expression.Lambda<Func<T, bool>>(body, parameter);
            }
            catch (Exception)
            {
                // En caso de error, retornar expresión que siempre es true
                // En producción, loguear el error
                return x => true;
            }
        }

        /// <summary>
        /// Crea expresión de igualdad.
        /// </summary>
        private static Expression? CreateEqualsExpression(MemberExpression property, string value, Type propertyType)
        {
            object? convertedValue = ConvertValue(value, propertyType);
            if (convertedValue == null) return null;

            Expression valueExpression = Expression.Constant(convertedValue, propertyType);
            return Expression.Equal(property, valueExpression);
        }

        /// <summary>
        /// Crea expresión de desigualdad.
        /// </summary>
        private static Expression? CreateNotEqualsExpression(MemberExpression property, string value, Type propertyType)
        {
            object? convertedValue = ConvertValue(value, propertyType);
            if (convertedValue == null) return null;

            Expression valueExpression = Expression.Constant(convertedValue, propertyType);
            return Expression.NotEqual(property, valueExpression);
        }

        /// <summary>
        /// Crea expresión Contains para strings.
        /// </summary>
        private static Expression? CreateContainsExpression(MemberExpression property, string value)
        {
            if (property.Type != typeof(string))
                return null;

            MethodInfo? containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            if (containsMethod == null) return null;

            Expression valueExpression = Expression.Constant(value);

            // Manejo de nulls: property != null && property.Contains(value)
            Expression notNullExpression = Expression.NotEqual(property, Expression.Constant(null));
            Expression containsExpression = Expression.Call(property, containsMethod, valueExpression);

            return Expression.AndAlso(notNullExpression, containsExpression);
        }

        /// <summary>
        /// Crea expresión StartsWith para strings.
        /// </summary>
        private static Expression? CreateStartsWithExpression(MemberExpression property, string value)
        {
            if (property.Type != typeof(string))
                return null;

            MethodInfo? startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            if (startsWithMethod == null) return null;

            Expression valueExpression = Expression.Constant(value);
            Expression notNullExpression = Expression.NotEqual(property, Expression.Constant(null));
            Expression startsWithExpression = Expression.Call(property, startsWithMethod, valueExpression);

            return Expression.AndAlso(notNullExpression, startsWithExpression);
        }

        /// <summary>
        /// Crea expresión EndsWith para strings.
        /// </summary>
        private static Expression? CreateEndsWithExpression(MemberExpression property, string value)
        {
            if (property.Type != typeof(string))
                return null;

            MethodInfo? endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
            if (endsWithMethod == null) return null;

            Expression valueExpression = Expression.Constant(value);
            Expression notNullExpression = Expression.NotEqual(property, Expression.Constant(null));
            Expression endsWithExpression = Expression.Call(property, endsWithMethod, valueExpression);

            return Expression.AndAlso(notNullExpression, endsWithExpression);
        }

        /// <summary>
        /// Crea expresiones de comparación (mayor, menor, etc).
        /// </summary>
        private static Expression? CreateComparisonExpression(
            MemberExpression property,
            string value,
            Type propertyType,
            Func<Expression, Expression, Expression> comparison)
        {
            object? convertedValue = ConvertValue(value, propertyType);
            if (convertedValue == null) return null;

            Expression valueExpression = Expression.Constant(convertedValue, propertyType);
            return comparison(property, valueExpression);
        }

        /// <summary>
        /// Crea expresión para verificar null o vacío.
        /// </summary>
        private static Expression CreateIsNullOrEmptyExpression(MemberExpression property, Type propertyType)
        {
            Expression nullExpression = Expression.Equal(property, Expression.Constant(null));

            if (propertyType == typeof(string))
            {
                MethodInfo? isNullOrEmptyMethod = typeof(string).GetMethod("IsNullOrEmpty", new[] { typeof(string) });
                if (isNullOrEmptyMethod != null)
                {
                    return Expression.Call(null, isNullOrEmptyMethod, property);
                }
            }

            return nullExpression;
        }

        /// <summary>
        /// Crea expresión para verificar NO null ni vacío.
        /// </summary>
        private static Expression CreateIsNotNullOrEmptyExpression(MemberExpression property, Type propertyType)
        {
            Expression isNullOrEmpty = CreateIsNullOrEmptyExpression(property, propertyType);
            return Expression.Not(isNullOrEmpty);
        }

        /// <summary>
        /// Convierte string a tipo de destino.
        /// </summary>
        private static object? ConvertValue(string value, Type targetType)
        {
            try
            {
                // Manejo de tipos nullable
                Type actualType = Nullable.GetUnderlyingType(targetType) ?? targetType;

                // Conversión según tipo
                if (actualType == typeof(string))
                    return value;
                if (actualType == typeof(int))
                    return int.Parse(value);
                if (actualType == typeof(long))
                    return long.Parse(value);
                if (actualType == typeof(decimal))
                    return decimal.Parse(value);
                if (actualType == typeof(double))
                    return double.Parse(value);
                if (actualType == typeof(float))
                    return float.Parse(value);
                if (actualType == typeof(bool))
                    return bool.Parse(value);
                if (actualType == typeof(DateTime))
                    return DateTime.Parse(value);
                if (actualType == typeof(Guid))
                    return Guid.Parse(value);
                if (actualType.IsEnum)
                    return Enum.Parse(actualType, value, true);

                // Intento genérico de conversión
                return Convert.ChangeType(value, actualType);
            }
            catch
            {
                return null;
            }
        }
    }
}