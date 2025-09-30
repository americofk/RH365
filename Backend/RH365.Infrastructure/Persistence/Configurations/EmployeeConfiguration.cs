// ============================================================================
// Archivo: EmployeeConfiguration.cs
// Capa: RH365.Infrastructure
// Ruta: Infrastructure/Persistence/Configurations/EmployeeConfiguration.cs
// Descripción: Configuración EF Core para la entidad Employee.
//   - Tabla: [dbo].[Employees]
//   - PK real: RecID (secuencia global dbo.RecId; se configura en ApplicationDbContext).
//   - ID legible (string): 'EMP-'+RIGHT(...,8) con secuencia dbo.EmployeesId (DEFAULT).
//   - Índice único: (DataareaID, EmployeeCode).
//   - Defaults y validaciones: EmployeeStatus = 1; flags bool a 0; checks de fechas/horas.
//   - Tipos y longitudes según buenas prácticas (ISO 27001).
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de mapeo para Employees.
    /// </summary>
    public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            // Tabla y esquema
            builder.ToTable("Employees", "dbo");

            // ---------------------------
            // Propiedades obligatorias
            // ---------------------------
            builder.Property(e => e.EmployeeCode)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(e => e.Name)
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(e => e.LastName)
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(e => e.Nss)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(e => e.Ars)
                   .HasMaxLength(80)
                   .IsRequired();

            builder.Property(e => e.Afp)
                   .HasMaxLength(80)
                   .IsRequired();

            builder.Property(e => e.Nationality)
                   .HasMaxLength(80);

            builder.Property(e => e.PersonalTreatment)
                   .HasMaxLength(30);

            // Fechas y horas
            builder.Property(e => e.BirthDate)
                   .HasColumnType("datetime2")
                   .IsRequired();

            builder.Property(e => e.AdmissionDate)
                   .HasColumnType("datetime2")
                   .IsRequired();

            builder.Property(e => e.StartWorkDate)
                   .HasColumnType("datetime2")
                   .IsRequired();

            builder.Property(e => e.EndWorkDate)
                   .HasColumnType("datetime2");

            // Horarios (TimeOnly → SQL 'time')
            builder.Property(e => e.WorkFrom).HasColumnType("time");
            builder.Property(e => e.WorkTo).HasColumnType("time");
            builder.Property(e => e.BreakWorkFrom).HasColumnType("time");
            builder.Property(e => e.BreakWorkTo).HasColumnType("time");

            // Enums como int (por claridad explícita)
            builder.Property(e => e.Gender).HasConversion<int>();
            builder.Property(e => e.MaritalStatus).HasConversion<int>();
            builder.Property(e => e.EmployeeType).HasConversion<int>();
            builder.Property(e => e.PayMethod).HasConversion<int>();
            builder.Property(e => e.WorkStatus).HasConversion<int>();
            builder.Property(e => e.EmployeeAction).HasConversion<int>();

            // Banderas booleanas con DEFAULT 0
            builder.Property(e => e.HomeOffice).HasDefaultValue(false).IsRequired();
            builder.Property(e => e.OwnCar).HasDefaultValue(false).IsRequired();
            builder.Property(e => e.HasDisability).HasDefaultValue(false).IsRequired();
            builder.Property(e => e.ApplyForOvertime).HasDefaultValue(false).IsRequired();
            builder.Property(e => e.IsFixedWorkCalendar).HasDefaultValue(false).IsRequired();

            // Estado del empleado: NOT NULL + DEFAULT 1 (activo)
            builder.Property(e => e.EmployeeStatus)
                   .HasDefaultValue(true)
                   .IsRequired();

            // Age y DependentsNumbers no negativos
            builder.Property(e => e.Age).IsRequired();
            builder.Property(e => e.DependentsNumbers).IsRequired();

            // ---------------------------
            // Sombra: ID legible (string)
            // ---------------------------
            // Generado por BD con secuencia propia dbo.EmployeesId (8 dígitos, padding con ceros)
            builder.Property<string>("ID")
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("('EMP-' + RIGHT('00000000' + CAST(NEXT VALUE FOR dbo.EmployeesId AS VARCHAR(8)), 8))");

            // ---------------------------
            // Índices y unicidad
            // ---------------------------
            builder.HasIndex(e => new { e.DataareaID, e.EmployeeCode })
                   .IsUnique()
                   .HasDatabaseName("UX_Employees_Dataarea_EmployeeCode");

            builder.HasIndex(e => e.CountryRecId).HasDatabaseName("IX_Employees_CountryRecId");
            builder.HasIndex(e => e.DisabilityTypeRecId).HasDatabaseName("IX_Employees_DisabilityType");
            builder.HasIndex(e => e.EducationLevelRecId).HasDatabaseName("IX_Employees_EducationLevel");
            builder.HasIndex(e => e.OccupationRecId).HasDatabaseName("IX_Employees_Occupation");
            builder.HasIndex(e => e.LocationRecId).HasDatabaseName("IX_Employees_Location");

            // ---------------------------
            // Check constraints (consistencia)
            // ---------------------------
            builder.ToTable(t =>
            {
                // Edad y dependientes >= 0
                t.HasCheckConstraint("CK_Employees_Age_NonNegative", "[Age] >= 0");
                t.HasCheckConstraint("CK_Employees_Dependents_NonNegative", "[DependentsNumbers] >= 0");

                // EndWorkDate debe ser >= StartWorkDate (cuando no es NULL)
                t.HasCheckConstraint("CK_Employees_EndWorkDate", "[EndWorkDate] IS NULL OR [EndWorkDate] >= [StartWorkDate]");

                // Horas válidas (cuando están informadas)
                t.HasCheckConstraint("CK_Employees_WorkHours",
                    "([WorkFrom] IS NULL OR [WorkTo] IS NULL OR [WorkFrom] < [WorkTo])");

                t.HasCheckConstraint("CK_Employees_BreakHours",
                    "([BreakWorkFrom] IS NULL OR [BreakWorkTo] IS NULL OR [BreakWorkFrom] < [BreakWorkTo])");
            });

            // Nota: Las FKs están modeladas como RecID (long) en la entidad; si agregas
            // navegaciones futuras (Country, DisabilityType, etc.) define aquí .HasOne().HasForeignKey().
        }
    }
}
