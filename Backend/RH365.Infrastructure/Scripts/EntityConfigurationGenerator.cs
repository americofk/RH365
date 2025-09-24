// ============================================================================
// Archivo: EntityConfigurationGenerator.cs
// Proyecto: RH365.Infrastructure (Script temporal)
// Ruta: RH365.Infrastructure/Scripts/EntityConfigurationGenerator.cs
// Descripción: Generador automático de configuraciones Entity Framework.
//   - Analiza todas las entidades por reflection
//   - Genera configuraciones básicas automáticamente
//   - Aplica convenciones estándar ISO 27001
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using RH365.Core.Domain.Common;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Scripts
{
    /// <summary>
    /// Script para generar masivamente las configuraciones de entidades.
    /// Ejecutar una sola vez para crear todos los archivos de configuración.
    /// </summary>
    public class EntityConfigurationGenerator
    {
        private readonly string _outputPath;
        private readonly Dictionary<string, string> _moduleMapping;

        public EntityConfigurationGenerator(string outputPath = @"C:\GeneratedConfigurations")
        {
            _outputPath = outputPath;
            _moduleMapping = InitializeModuleMapping();
        }

        /// <summary>
        /// Ejecuta la generación masiva de todas las configuraciones.
        /// </summary>
        public void GenerateAllConfigurations()
        {
            Console.WriteLine("🚀 Iniciando generación masiva de Entity Configurations...");

            // Crear directorio de salida
            Directory.CreateDirectory(_outputPath);

            // Obtener todas las entidades del dominio
            var entityTypes = GetAllEntityTypes();

            Console.WriteLine($"📊 Encontradas {entityTypes.Count} entidades para procesar...");

            foreach (var entityType in entityTypes)
            {
                try
                {
                    var configuration = GenerateEntityConfiguration(entityType);
                    var fileName = $"{entityType.Name}Configuration.cs";
                    var filePath = Path.Combine(_outputPath, fileName);

                    File.WriteAllText(filePath, configuration);
                    Console.WriteLine($"✅ Generado: {fileName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error generando {entityType.Name}: {ex.Message}");
                }
            }

            Console.WriteLine($"🎉 Generación completada. Archivos guardados en: {_outputPath}");
            Console.WriteLine("📋 Revisar y personalizar configuraciones según necesidades específicas.");
        }

        /// <summary>
        /// Obtiene todos los tipos de entidad del dominio.
        /// </summary>
        private List<Type> GetAllEntityTypes()
        {
            var assembly = typeof(Employee).Assembly; // RH365.Core

            return assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => typeof(BaseEntity).IsAssignableFrom(t))
                .Where(t => t != typeof(BaseEntity) &&
                           t != typeof(AuditableEntity) &&
                           t != typeof(AuditableCompanyEntity))
                .OrderBy(t => t.Name)
                .ToList();
        }

        /// <summary>
        /// Genera la configuración completa para una entidad específica.
        /// </summary>
        private string GenerateEntityConfiguration(Type entityType)
        {
            var sb = new StringBuilder();
            var tableName = GetTableName(entityType);
            var module = GetModuleName(entityType.Name);

            // Header
            sb.AppendLine($"// ============================================================================");
            sb.AppendLine($"// Archivo: {entityType.Name}Configuration.cs");
            sb.AppendLine($"// Proyecto: RH365.Infrastructure");
            sb.AppendLine($"// Ruta: RH365.Infrastructure/Persistence/Configurations/{module}/{entityType.Name}Configuration.cs");
            sb.AppendLine($"// Descripción: Configuración Entity Framework para {entityType.Name}.");
            sb.AppendLine($"//   - Mapeo de propiedades y relaciones");
            sb.AppendLine($"//   - Índices y restricciones de base de datos");
            sb.AppendLine($"//   - Cumplimiento ISO 27001");
            sb.AppendLine($"// ============================================================================");
            sb.AppendLine();

            // Usings
            sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            sb.AppendLine("using Microsoft.EntityFrameworkCore.Metadata.Builders;");
            sb.AppendLine("using RH365.Core.Domain.Entities;");
            sb.AppendLine();

            // Namespace
            sb.AppendLine($"namespace RH365.Infrastructure.Persistence.Configurations.{module}");
            sb.AppendLine("{");

            // Class declaration
            sb.AppendLine($"    /// <summary>");
            sb.AppendLine($"    /// Configuración Entity Framework para la entidad {entityType.Name}.");
            sb.AppendLine($"    /// </summary>");
            sb.AppendLine($"    public class {entityType.Name}Configuration : IEntityTypeConfiguration<{entityType.Name}>");
            sb.AppendLine("    {");

            // Configure method
            sb.AppendLine($"        public void Configure(EntityTypeBuilder<{entityType.Name}> builder)");
            sb.AppendLine("        {");
            sb.AppendLine($"            // Mapeo a tabla");
            sb.AppendLine($"            builder.ToTable(\"{tableName}\");");
            sb.AppendLine();

            // Generate property configurations
            GeneratePropertyConfigurations(sb, entityType);

            // Generate relationship configurations
            GenerateRelationshipConfigurations(sb, entityType);

            // Generate indexes
            GenerateIndexConfigurations(sb, entityType);

            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        /// <summary>
        /// Genera configuraciones de propiedades.
        /// </summary>
        private void GeneratePropertyConfigurations(StringBuilder sb, Type entityType)
        {
            sb.AppendLine("            // Configuración de propiedades");

            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => !p.PropertyType.IsGenericType ||
                           !typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType))
                .Where(p => p.CanWrite && p.CanRead)
                .OrderBy(p => p.Name);

            foreach (var prop in properties)
            {
                GeneratePropertyConfiguration(sb, prop);
            }

            sb.AppendLine();
        }

        /// <summary>
        /// Genera configuración para una propiedad específica.
        /// </summary>
        private void GeneratePropertyConfiguration(StringBuilder sb, PropertyInfo property)
        {
            var propType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

            // Skip inherited audit properties - handled by base configuration
            if (IsAuditProperty(property.Name))
                return;

            if (propType == typeof(string))
            {
                var maxLength = GetStringMaxLength(property.Name);
                var isRequired = !IsNullable(property);

                sb.Append($"            builder.Property(e => e.{property.Name})");

                if (isRequired)
                    sb.Append($".IsRequired()");

                sb.Append($".HasMaxLength({maxLength})");
                sb.AppendLine($".HasColumnName(\"{property.Name}\");");
            }
            else if (propType == typeof(decimal))
            {
                var precision = GetDecimalPrecision(property.Name);
                sb.AppendLine($"            builder.Property(e => e.{property.Name})" +
                             $".HasPrecision({precision.precision}, {precision.scale})" +
                             $".HasColumnName(\"{property.Name}\");");
            }
            else if (propType == typeof(DateTime) || propType == typeof(TimeOnly))
            {
                sb.AppendLine($"            builder.Property(e => e.{property.Name})" +
                             $".HasColumnType(\"datetime2\")" +
                             $".HasColumnName(\"{property.Name}\");");
            }
            else if (propType.IsEnum)
            {
                sb.AppendLine($"            builder.Property(e => e.{property.Name})" +
                             $".HasConversion<int>()" +
                             $".HasColumnName(\"{property.Name}\");");
            }
            else if (propType == typeof(byte[]))
            {
                sb.AppendLine($"            builder.Property(e => e.{property.Name})" +
                             $".HasColumnType(\"varbinary(max)\")" +
                             $".HasColumnName(\"{property.Name}\");");
            }
            else
            {
                sb.AppendLine($"            builder.Property(e => e.{property.Name})" +
                             $".HasColumnName(\"{property.Name}\");");
            }
        }

        /// <summary>
        /// Genera configuraciones de relaciones.
        /// </summary>
        private void GenerateRelationshipConfigurations(StringBuilder sb, Type entityType)
        {
            var navigationProperties = entityType.GetProperties()
                .Where(p => p.PropertyType.IsGenericType &&
                           typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType) ||
                           (p.PropertyType.IsClass && p.PropertyType != typeof(string) &&
                            typeof(BaseEntity).IsAssignableFrom(p.PropertyType)))
                .ToList();

            if (navigationProperties.Any())
            {
                sb.AppendLine("            // Configuración de relaciones");

                foreach (var nav in navigationProperties)
                {
                    GenerateNavigationConfiguration(sb, nav, entityType);
                }
                sb.AppendLine();
            }
        }

        /// <summary>
        /// Genera configuración para propiedades de navegación.
        /// </summary>
        private void GenerateNavigationConfiguration(StringBuilder sb, PropertyInfo navigation, Type entityType)
        {
            if (navigation.PropertyType.IsGenericType) // Collection
            {
                var relatedType = navigation.PropertyType.GetGenericArguments()[0];
                sb.AppendLine($"            builder.HasMany(e => e.{navigation.Name})");
                sb.AppendLine($"                .WithOne(d => d.{GetInverseProperty(entityType, relatedType)})");
                sb.AppendLine($"                .HasForeignKey(d => d.{GetForeignKey(entityType)})");
                sb.AppendLine($"                .OnDelete(DeleteBehavior.ClientSetNull);");
            }
            else // Reference
            {
                if (navigation.Name.EndsWith("RefRec"))
                {
                    var fkProperty = navigation.Name.Replace("RefRec", "RefRecID");
                    sb.AppendLine($"            builder.HasOne(e => e.{navigation.Name})");
                    sb.AppendLine($"                .WithMany()");
                    sb.AppendLine($"                .HasForeignKey(e => e.{fkProperty})");
                    sb.AppendLine($"                .OnDelete(DeleteBehavior.ClientSetNull);");
                }
            }
        }

        /// <summary>
        /// Genera configuraciones de índices.
        /// </summary>
        private void GenerateIndexConfigurations(StringBuilder sb, Type entityType)
        {
            sb.AppendLine("            // Índices");

            // Índice para códigos únicos
            var codeProperties = entityType.GetProperties()
                .Where(p => p.Name.EndsWith("Code") && p.PropertyType == typeof(string))
                .ToList();

            foreach (var codeProp in codeProperties)
            {
                sb.AppendLine($"            builder.HasIndex(e => new {{ e.{codeProp.Name}, e.DataareaID }})");
                sb.AppendLine($"                .HasDatabaseName(\"IX_{entityType.Name}_{codeProp.Name}_DataareaID\")");
                sb.AppendLine($"                .IsUnique();");
            }

            // Índices para foreign keys
            var fkProperties = entityType.GetProperties()
                .Where(p => p.Name.EndsWith("RefRecID") && p.PropertyType == typeof(long))
                .ToList();

            foreach (var fk in fkProperties)
            {
                sb.AppendLine($"            builder.HasIndex(e => e.{fk.Name})");
                sb.AppendLine($"                .HasDatabaseName(\"IX_{entityType.Name}_{fk.Name}\");");
            }
        }

        #region Helper Methods

        private Dictionary<string, string> InitializeModuleMapping()
        {
            return new Dictionary<string, string>
            {
                // System
                {"User", "System"}, {"UserImage", "System"}, {"MenusApp", "System"},
                {"MenuAssignedToUser", "System"}, {"FormatCode", "System"}, {"GeneralConfig", "System"},
                
                // Organization
                {"Company", "Organization"}, {"CompaniesAssignedToUser", "Organization"},
                {"Department", "Organization"}, {"Position", "Organization"},
                {"PositionRequirement", "Organization"}, {"Job", "Organization"},
                
                // Employee
                {"Employee", "Employee"}, {"EmployeeBankAccount", "Employee"},
                {"EmployeeContactsInf", "Employee"}, {"EmployeeDepartment", "Employee"},
                {"EmployeeDocument", "Employee"}, {"EmployeeHistory", "Employee"},
                {"EmployeeImage", "Employee"}, {"EmployeePosition", "Employee"},
                {"EmployeesAddress", "Employee"}, {"EmployeeWorkCalendar", "Employee"},
                {"EmployeeWorkControlCalendar", "Employee"}, {"DisabilityType", "Employee"},
                {"EducationLevel", "Employee"}, {"Occupation", "Employee"},
                
                // Payroll
                {"Payroll", "Payroll"}, {"PayCycle", "Payroll"}, {"PayrollsProcess", "Payroll"},
                {"PayrollProcessAction", "Payroll"}, {"PayrollProcessDetail", "Payroll"},
                {"EarningCode", "Payroll"}, {"DeductionCode", "Payroll"}, {"EmployeeEarningCode", "Payroll"},
                {"EmployeeDeductionCode", "Payroll"}, {"EmployeeExtraHour", "Payroll"},
                {"EmployeeLoan", "Payroll"}, {"EmployeeLoanHistory", "Payroll"},
                {"EmployeeTax", "Payroll"}, {"Loan", "Payroll"}, {"Taxis", "Payroll"}, {"TaxDetail", "Payroll"},
                
                // Training
                {"Course", "Training"}, {"CourseEmployee", "Training"}, {"CourseInstructor", "Training"},
                {"CourseLocation", "Training"}, {"CoursePosition", "Training"}, {"CourseType", "Training"},
                {"ClassRoom", "Training"}, {"Instructor", "Training"},
                
                // General
                {"Country", "General"}, {"Currency", "General"}, {"Province", "General"},
                {"CalendarHoliday", "General"}, {"Project", "General"}, {"ProjectCategory", "General"},
                
                // Audit
                {"AuditLog", "Audit"}
            };
        }

        private string GetModuleName(string entityName)
        {
            return _moduleMapping.TryGetValue(entityName, out var module) ? module : "General";
        }

        private string GetTableName(Type entityType)
        {
            // Por convención, el nombre de la tabla es igual al nombre de la entidad
            return entityType.Name;
        }

        private int GetStringMaxLength(string propertyName)
        {
            // Definir longitudes estándar según el tipo de campo
            return propertyName.ToLower() switch
            {
                var name when name.Contains("code") => 50,
                var name when name.Contains("email") => 255,
                var name when name.Contains("phone") => 20,
                var name when name.Contains("name") => 255,
                var name when name.Contains("description") => 500,
                var name when name.Contains("comment") => 500,
                var name when name.Contains("observations") => 500,
                "dataareaId" => 10,
                "createdby" => 50,
                "modifiedby" => 50,
                "id" => 50,
                _ => 255
            };
        }

        private (int precision, int scale) GetDecimalPrecision(string propertyName)
        {
            // Definir precisión según el tipo de campo monetario
            return propertyName.ToLower() switch
            {
                var name when name.Contains("amount") => (18, 4),
                var name when name.Contains("percent") => (5, 2),
                var name when name.Contains("index") => (18, 4),
                var name when name.Contains("multiply") => (18, 4),
                _ => (18, 2)
            };
        }

        private bool IsNullable(PropertyInfo property)
        {
            return Nullable.GetUnderlyingType(property.PropertyType) != null ||
                   property.PropertyType == typeof(string) &&
                   property.GetCustomAttribute<System.ComponentModel.DataAnnotations.RequiredAttribute>() == null;
        }

        private bool IsAuditProperty(string propertyName)
        {
            var auditProperties = new[] { "RecID", "ID", "CreatedBy", "CreatedOn",
                                        "ModifiedBy", "ModifiedOn", "DataareaID",
                                        "Observations", "RowVersion" };
            return auditProperties.Contains(propertyName);
        }

        private string GetForeignKey(Type parentType)
        {
            return $"{parentType.Name}RefRecID";
        }

        private string GetInverseProperty(Type parentType, Type childType)
        {
            // Buscar la propiedad de navegación inversa
            var props = childType.GetProperties()
                .Where(p => p.PropertyType == parentType)
                .ToList();

            return props.FirstOrDefault()?.Name ?? $"{parentType.Name}RefRec";
        }

        #endregion

        /// <summary>
        /// Método principal para ejecutar desde consola o test.
        /// </summary>
        public static void Main(string[] args)
        {
            var generator = new EntityConfigurationGenerator();
            generator.GenerateAllConfigurations();

            Console.WriteLine("\n🔄 Presiona cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}