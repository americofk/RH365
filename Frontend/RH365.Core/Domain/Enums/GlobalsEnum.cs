// ============================================================================
// Archivo: GlobalsEnum.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Enums/GlobalsEnum.cs
// Descripción:
//   - Catálogo centralizado de enumeraciones del sistema
//   - Define todos los valores de listas desplegables
//   - Estandarización ISO 27001: Documentación completa y trazabilidad
//   - Todos los valores y descripciones en español
// Autor: Sistema RH365
// Fecha: 2025
// ============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RH365.Core.Domain.Models.Enums
{
    /// <summary>
    /// Frecuencia de pago para nómina
    /// ISO 27001: Control de frecuencias de procesamiento de pagos
    /// </summary>
    public enum FrecuenciaPago
    {
        [Display(Name = "Diario")]
        Diario = 0,

        [Display(Name = "Semanal")]
        Semanal = 1,

        [Display(Name = "Bisemanal")]
        Bisemanal = 2,

        [Display(Name = "Quincenal")]
        Quincenal = 3,

        [Display(Name = "Mensual")]
        Mensual = 4,

        [Display(Name = "Trimestral")]
        Trimestral = 5,

        [Display(Name = "Cuatrimestral")]
        Cuatrimestral = 6,

        [Display(Name = "Semestral")]
        Semestral = 7,

        [Display(Name = "Anual")]
        Anual = 8
    }

    /// <summary>
    /// Acciones de empleado para gestión de recursos humanos
    /// ISO 27001: Registro de cambios en el estado del empleado
    /// </summary>
    public enum AccionEmpleado
    {
        [Display(Name = "Ninguno")]
        Ninguno = 0,

        [Display(Name = "Desahucio")]
        Desahucio = 1,

        [Display(Name = "Renuncia")]
        Renuncia = 2,

        [Display(Name = "Término de Contrato Temporal")]
        TerminoContratoTemporal = 3,

        [Display(Name = "Muerte")]
        Muerte = 4,

        [Display(Name = "Transferencia")]
        Transferencia = 5,

        [Display(Name = "Término Contrato Periodo Probatorio")]
        TerminoContratoPeriodoProbatorio = 6,

        [Display(Name = "Promoción")]
        Promocion = 7,

        [Display(Name = "Transferencia de Departamento")]
        TransferenciaDepartamento = 8,

        [Display(Name = "Transferencia entre Empresas")]
        TransferenciaEmpresas = 9,

        [Display(Name = "Contrato Temporero")]
        ContratoTemporero = 10,

        [Display(Name = "Término de Contrato")]
        TerminoContrato = 11,

        [Display(Name = "Despido")]
        Despido = 12,

        [Display(Name = "Enfermedad")]
        Enfermedad = 13
    }

    /// <summary>
    /// Estados del período de nómina
    /// ISO 27001: Control de estados para procesamiento de pagos
    /// </summary>
    public enum EstadoPeriodo
    {
        [Display(Name = "Abierto")]
        Abierto = 0,

        [Display(Name = "Procesado")]
        Procesado = 1,

        [Display(Name = "Pagado")]
        Pagado = 2,

        [Display(Name = "Registrado")]
        Registrado = 3
    }

    /// <summary>
    /// Tipos de administrador del sistema
    /// ISO 27001: Control de acceso basado en roles
    /// </summary>
    public enum TipoAdministrador
    {
        [Display(Name = "Administrador Local")]
        AdministradorLocal = 0,

        [Display(Name = "Administrador Global")]
        AdministradorGlobal = 1,

        [Display(Name = "Usuario")]
        Usuario = 2
    }

    /// <summary>
    /// Estados de cursos de capacitación
    /// ISO 27001: Seguimiento de formación del personal
    /// </summary>
    public enum EstadoCurso
    {
        [Display(Name = "Creado")]
        Creado = 0,

        [Display(Name = "En Proceso")]
        EnProceso = 1,

        [Display(Name = "Cerrado")]
        Cerrado = 2
    }

    /// <summary>
    /// Clasificación interno/externo
    /// ISO 27001: Identificación de recursos internos y externos
    /// </summary>
    public enum InternoExterno
    {
        [Display(Name = "Interno")]
        Interno = 0,

        [Display(Name = "Externo")]
        Externo = 1
    }

    /// <summary>
    /// Género del empleado
    /// ISO 27001: Datos demográficos para reportes de cumplimiento
    /// </summary>
    public enum Genero
    {
        [Display(Name = "Masculino")]
        Masculino = 0,

        [Display(Name = "Femenino")]
        Femenino = 1,

        [Display(Name = "No especificado")]
        NoEspecificado = 2
    }

    /// <summary>
    /// Estado civil del empleado
    /// ISO 27001: Información personal para beneficios y deducciones
    /// </summary>
    public enum EstadoCivil
    {
        [Display(Name = "Casado/a")]
        Casado = 0,

        [Display(Name = "Soltero/a")]
        Soltero = 1,

        [Display(Name = "Viudo/a")]
        Viudo = 2,

        [Display(Name = "Divorciado/a")]
        Divorciado = 3,

        [Display(Name = "Separado/a")]
        Separado = 4
    }

    /// <summary>
    /// Tipo de empleado
    /// ISO 27001: Clasificación laboral para control de acceso
    /// </summary>
    public enum TipoEmpleado
    {
        [Display(Name = "Empleado")]
        Empleado = 0,

        [Display(Name = "Contratista")]
        Contratista = 1
    }

    /// <summary>
    /// Tipo de contacto
    /// ISO 27001: Medios de comunicación con el empleado
    /// </summary>
    public enum TipoContacto
    {
        [Display(Name = "Celular")]
        Celular = 0,

        [Display(Name = "Correo Electrónico")]
        Correo = 1,

        [Display(Name = "Teléfono")]
        Telefono = 2,

        [Display(Name = "Otro")]
        Otro = 3
    }

    /// <summary>
    /// Base de índice para cálculos de nómina
    /// ISO 27001: Métodos de cálculo para pagos y deducciones
    /// </summary>
    public enum BaseIndice
    {
        [Display(Name = "Hora")]
        Hora = 0,

        [Display(Name = "Período de pago")]
        PeriodoPago = 1,

        [Display(Name = "Mensual")]
        Mensual = 2,

        [Display(Name = "Anual")]
        Anual = 3,

        [Display(Name = "Monto fijo")]
        MontoFijo = 4,

        [Display(Name = "Retroactivo")]
        Retroactivo = 5,

        [Display(Name = "Índice salarial estándar")]
        IndiceSalarialEstandar = 6,

        [Display(Name = "Porcentaje de ganancia")]
        PorcentajeGanancia = 7,

        [Display(Name = "Horas de trabajo")]
        HorasTrabajo = 8
    }

    /// <summary>
    /// Estado laboral del empleado
    /// ISO 27001: Control de estado del personal
    /// </summary>
    public enum EstadoLaboral
    {
        [Display(Name = "Candidato")]
        Candidato = 0,

        [Display(Name = "Despedido")]
        Despedido = 1,

        [Display(Name = "Empleado")]
        Empleado = 2,

        [Display(Name = "Deshabilitado")]
        Deshabilitado = 3
    }

    /// <summary>
    /// Base de índice simplificada (hora y monto fijo)
    /// ISO 27001: Cálculos simplificados de nómina
    /// </summary>
    public enum BaseIndiceDos
    {
        [Display(Name = "Hora")]
        Hora = 0,

        [Display(Name = "Monto fijo")]
        MontoFijo = 4
    }

    /// <summary>
    /// Tipo de acción de nómina
    /// ISO 27001: Control de deducciones y contribuciones
    /// </summary>
    public enum AccionNomina
    {
        [Display(Name = "Deducción")]
        Deduccion = 0,

        [Display(Name = "Contribución")]
        Contribucion = 1,

        [Display(Name = "Ambos")]
        Ambos = 2
    }

    /// <summary>
    /// Tipo de cuenta bancaria
    /// ISO 27001: Información bancaria para transferencias
    /// </summary>
    public enum TipoCuenta
    {
        [Display(Name = "Ahorro")]
        Ahorro = 0,

        [Display(Name = "Corriente")]
        Corriente = 1
    }

    /// <summary>
    /// Tipo de documento de identificación
    /// ISO 27001: Documentación legal del empleado
    /// </summary>
    public enum TipoDocumento
    {
        [Display(Name = "Cédula")]
        Cedula = 0,

        [Display(Name = "Pasaporte")]
        Pasaporte = 1,

        [Display(Name = "Residencia")]
        Residencia = 2,

        [Display(Name = "Licencia de conducir")]
        LicenciaConducir = 3
    }

    /// <summary>
    /// Estado del proceso de nómina
    /// ISO 27001: Flujo de trabajo del procesamiento de pagos
    /// </summary>
    public enum EstadoProcesoNomina
    {
        [Display(Name = "Creada")]
        Creada = 0,

        [Display(Name = "Procesada")]
        Procesada = 1,

        [Display(Name = "Calculada")]
        Calculada = 2,

        [Display(Name = "Pagada")]
        Pagada = 3,

        [Display(Name = "Cerrada")]
        Cerrada = 4,

        [Display(Name = "Cancelada")]
        Cancelada = 5
    }

    /// <summary>
    /// Opciones de listas desplegables del sistema
    /// ISO 27001: Catálogo de entidades para selección
    /// </summary>
    public enum OpcionesListaSeleccion
    {
        [Display(Name = "Código de formato")]
        CodigoFormato = 0,

        [Display(Name = "Moneda")]
        Moneda = 1,

        [Display(Name = "Nómina")]
        Nomina = 2,

        [Display(Name = "Proyecto")]
        Proyecto = 3,

        [Display(Name = "Categoría de proyecto")]
        CategoriaProyecto = 4,

        [Display(Name = "Empresa")]
        Empresa = 5,

        [Display(Name = "Ciclos de pago")]
        CiclosPago = 6,

        [Display(Name = "Departamento")]
        Departamento = 7,

        [Display(Name = "Código de deducción")]
        CodigoDeduccion = 8,

        [Display(Name = "Préstamo")]
        Prestamo = 9,

        [Display(Name = "Impuesto")]
        Impuesto = 10,

        [Display(Name = "Trabajo")]
        Trabajo = 11,

        [Display(Name = "Posición")]
        Posicion = 12,

        [Display(Name = "Código de ganancia")]
        CodigoGanancia = 13,

        [Display(Name = "Código de ganancia por horas")]
        CodigoGananciaHoras = 14,

        [Display(Name = "Tipo de curso")]
        TipoCurso = 15,

        [Display(Name = "Aula")]
        Aula = 16,

        [Display(Name = "Curso padre")]
        CursoPadre = 17,

        [Display(Name = "Instructor")]
        Instructor = 18,

        [Display(Name = "Empleado")]
        Empleado = 19,

        [Display(Name = "Código de ganancia adicional")]
        CodigoGananciaAdicional = 20,

        [Display(Name = "Nivel educativo")]
        NivelEducativo = 21,

        [Display(Name = "Tipo de discapacidad")]
        TipoDiscapacidad = 22,

        [Display(Name = "Ocupación")]
        Ocupacion = 23,

        [Display(Name = "País")]
        Pais = 24,

        [Display(Name = "Posición vacante")]
        PosicionVacante = 25,

        [Display(Name = "Provincia")]
        Provincia = 26
    }

    /// <summary>
    /// Estado de horas extras
    /// ISO 27001: Control de horas extraordinarias
    /// </summary>
    public enum EstadoHoraExtra
    {
        [Display(Name = "Abierta")]
        Abierta = 0,

        [Display(Name = "Pagada")]
        Pagada = 1
    }

    /// <summary>
    /// Método de pago
    /// ISO 27001: Medios de pago a empleados
    /// </summary>
    public enum MetodoPago
    {
        [Display(Name = "Efectivo")]
        Efectivo = 0,

        [Display(Name = "Transferencia")]
        Transferencia = 1
    }

    /// <summary>
    /// Tipos de reporte DGT (Dirección General de Trabajo)
    /// ISO 27001: Reportes gubernamentales obligatorios
    /// </summary>
    public enum TipoDGT
    {
        [Display(Name = "DGT-2")]
        Dgt2 = 0,

        [Display(Name = "DGT-3")]
        Dgt3 = 1,

        [Display(Name = "DGT-4")]
        Dgt4 = 2,

        [Display(Name = "DGT-5")]
        Dgt5 = 3,

        [Display(Name = "DGT-9")]
        Dgt9 = 4,

        [Display(Name = "DGT-12")]
        Dgt12 = 5,

        [Display(Name = "TSS")]
        TSS = 6
    }

    /// <summary>
    /// Tipo de acción de nómina
    /// ISO 27001: Categorización de transacciones de nómina
    /// </summary>
    public enum TipoAccionNomina
    {
        [Display(Name = "Ganancia")]
        Ganancia = 0,

        [Display(Name = "Deducción")]
        Deduccion = 1,

        [Display(Name = "Impuesto")]
        Impuesto = 2,

        [Display(Name = "Préstamo")]
        Prestamo = 3,

        [Display(Name = "Cooperativa")]
        Cooperativa = 4,

        [Display(Name = "Contribución")]
        Contribucion = 5,

        [Display(Name = "Horas extras")]
        HorasExtras = 6
    }

    /// <summary>
    /// Base de índice para deducciones
    /// ISO 27001: Métodos de cálculo para deducciones
    /// </summary>
    public enum BaseIndiceDeduccion
    {
        [Display(Name = "Monto fijo")]
        MontoFijo = 4,

        [Display(Name = "Porcentaje de ganancia")]
        PorcentajeGanancia = 7
    }

    /// <summary>
    /// Entidades para procesamiento por lotes
    /// ISO 27001: Procesamiento masivo de datos
    /// </summary>
    public enum EntidadLote
    {
        [Display(Name = "Empleados")]
        Empleado = 0,

        [Display(Name = "Dirección de empleados")]
        DireccionEmpleado = 1,

        [Display(Name = "Contacto de empleados")]
        ContactoEmpleado = 2,

        [Display(Name = "Documentos de empleados")]
        DocumentosEmpleado = 3,

        [Display(Name = "Horas extras de empleados")]
        HorasExtrasEmpleado = 4,

        [Display(Name = "Ganancias de empleados")]
        GananciasEmpleado = 5,

        [Display(Name = "Préstamos de empleados")]
        PrestamosEmpleado = 6,

        [Display(Name = "Bancos de empleados")]
        BancosEmpleado = 7,

        [Display(Name = "Impuestos de empleados")]
        ImpuestosEmpleado = 8,

        [Display(Name = "Deducciones de empleados")]
        DeduccionesEmpleado = 9,

        [Display(Name = "Horarios de empleados")]
        HorariosEmpleado = 10,

        [Display(Name = "Control asistencia de empleados")]
        ControlAsistenciaEmpleado = 11
    }

    /// <summary>
    /// Estado del control de trabajo
    /// ISO 27001: Control de asistencia y horas trabajadas
    /// </summary>
    public enum EstadoControlTrabajo
    {
        [Display(Name = "Pendiente")]
        Pendiente = 0,

        [Display(Name = "Pagado")]
        Pagado = 1
    }
}