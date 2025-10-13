using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RH365.Core.Domain.Models.Enums
{
    public class GlobalsEnum
    {
        public enum PayFrecuency
        {
            Diario = 0,
            Semanal = 1,
            Bisemanal = 2,
            Quincenal = 3,
            Mensual = 4,
            Trimestral = 5,
            Cuatrimestral = 6,
            Semestral = 7,
            Anual = 8
        }
    }
    public enum EmployeeAction
    {
        Ninguno = 0,
        Desahucio = 1,
        Renuncia = 2,
        TerminoContratoTemporal = 3,
        Muerte = 4,
        Transferencia = 5,
        TerminoContratoPeriodoProbatorio = 6,
        Promocion = 7,
        TransferenciaDepartamento = 8,
        TransferenciaEmpresas = 9,
        ContratoTemporero = 10,
        TerminoContrato = 11,
        Despido = 12,
        Enfermedad = 13
    }


    public enum StatusPeriod
    {
        [Display(Name = "Abierto")]
        Open = 0,
        [Display(Name = "Procesado")]
        Processed = 1,
        [Display(Name = "Pagado")]
        Paid = 2,
        [Display(Name = "Registrado")]
        Registered = 3
    }

    public enum AdminType
    {
        LocalAdmin = 0,
        GlobalAdmin = 1,
        User = 2
    }

    public enum CourseStatus
    {
        Created = 0,
        InProcess = 1,
        Closed = 2
    }

    public enum InternalExternal
    {
        [Display(Name = "Interno")]
        Internal = 0,
        [Display(Name = "Externo")]
        External = 1,
    }

    public enum Gender
    {
        [Display(Name = "Masculino")]
        Male = 0,
        [Display(Name = "Femenino")]
        Female = 1,
        [Display(Name = "No especificado")]
        NonSpecific = 2
    }

    public enum MaritalStatus
    {
        [Display(Name = "Casado/a")]
        Married = 0,
        [Display(Name = "Soltero/a")]
        Single = 1,
        [Display(Name = "Viudo/a")]
        Widowed = 2,
        [Display(Name = "Divorciado/a")]
        Divorced = 3,
        [Display(Name = "Separado/a")]
        Separated = 4
    }

    public enum EmployeeType
    {
        [Display(Name = "Empleado")]
        Employee = 0,
        [Display(Name = "Contratista")]
        Contractor = 1,
        //[Display(Name = "Pendiente por suspender")]
        //PendingSuspend = 2
    }

    public enum ContactType
    {
        [Display(Name = "Celular")]
        MobilePhone = 0,

        [Display(Name = "Correo")]
        Email = 1,

        [Display(Name = "Teléfono")]
        Phone = 2,

        [Display(Name = "Otro")]
        Otro = 3
    }

    public enum IndexBase
    {
        [Display(Name = "Hora")]
        Hour = 0, //Hora
        [Display(Name = "Período de pago")]
        PayPeriod = 1,// Periodo de pago
        [Display(Name = "Mensual")]
        Monthly = 2,//Mensual
        [Display(Name = "Anual")]
        Yearly = 3, //Anual
        [Display(Name = "Monto fijo")]
        FixedAmount = 4, //Monto fijo
        [Display(Name = "Retroactivo")]
        Retroactive = 5, //Retroactivo
        [Display(Name = "Indice salarial estándar")]
        StandardWageRate = 6, // Indice salarial estandar
        [Display(Name = "Porcentaje de ganancia")]
        EarningPercent = 7, // Porcenatge de ganancia
        [Display(Name = "Horas de trabajo")]
        EarningHours = 8 // Horas de trabajo
    }

    public enum WorkStatus
    {
        Candidate = 0,
        Dismissed = 1,
        Employ = 2,
        Disable = 3
    }

    public enum IndexBaseTwo
    {
        [Display(Name = "Hora")]
        Hour = 0, //Hora
        [Display(Name = "Monto fijo")]
        FixedAmount = 4, //Monto fijo

    }

    public enum PayrollAction
    {
        [Display(Name = "Deducción")]
        Deduction = 0,
        [Display(Name = "Contribución")]
        Contribution = 1,
        [Display(Name = "Ambos")]
        Both = 2
    }

    public enum PayFrecuency
    {
        [Display(Name = "Diario")]
        Diary = 0,
        [Display(Name = "Semanal")]
        Weekly = 1,
        [Display(Name = "Bisemanal")]
        TwoWeekly = 2,
        [Display(Name = "Quincenal")]
        BiWeekly = 3,
        [Display(Name = "Mensual")]
        Monthly = 4,
        [Display(Name = "Trimestral")]
        ThreeMonth = 5,
        [Display(Name = "Cuatrimestral")]
        FourMonth = 6,
        [Display(Name = "Semestral")]
        Biannual = 7,
        [Display(Name = "Anual")]
        Yearly = 8
    }

    public enum AccountType
    {
        [Display(Name = "Ahorro")]
        Ahorro = 0,
        [Display(Name = "Corriente")]
        Corriente = 1
    }

    public enum DocumentType
    {
        [Display(Name = "Cedula")]
        IdentificationCard = 0,
        [Display(Name = "Pasaporte")]
        Passport = 1,
        [Display(Name = "Residencia")]
        Residence = 2,
        [Display(Name = "Licencia de conducir")]
        DriverLicence = 3
    }

    public enum PayrollProcessStatus
    {
        [Display(Name = "Creada")]
        Created = 0,

        [Display(Name = "Procesada")]
        Processed = 1,

        [Display(Name = "Calculada")]
        Calculated = 2,

        [Display(Name = "Pagada")]
        Paid = 3,

        [Display(Name = "Cerrada")]
        Closed = 4,

        [Display(Name = "Cancelada")]
        Canceled = 5
    }

    public enum SelectListOptions
    {
        FormatCode = 0,
        Currency = 1,
        Payroll = 2,
        Project = 3,
        ProjCategory = 4,
        Company = 5,
        PayCycles = 6,
        Department = 7,
        DeductionCode = 8,
        Loan = 9,
        Tax = 10,
        Job = 11,
        Position = 12,
        EarningCode = 13,
        EarningCodehours = 14,
        CourseType = 15,
        ClassRoomId = 16,
        CourseParentId = 17,
        InstructorId = 18,
        EmployeeId = 19,
        EarningCodeEarning = 20,
        EducationLevel = 21,
        DisabilityType = 22,
        Occupation = 23,
        Country = 24,
        PositionVacant = 25,
        Province = 26

    }

    public enum StatusExtraHour
    {
        [Display(Name = "Abierta")]
        Open = 0,
        [Display(Name = "Pagada")]
        Pagada = 1
    }

    public enum PayMethod
    {

        [Display(Name = "Efectivo")]
        Cash = 0,
        [Display(Name = "Transferencia")]
        Transfer = 1,
    }
    public enum TypeDTG
    {

        Dgt2 = 0,
        Dgt3 = 1,
        Dgt4 = 2,
        Dgt5 = 3,
        Dgt9 = 4,
        Dgt12 = 5,
        TSS = 6
    }

    public enum PayrollActionType
    {
        [Display(Name = "Ganancia")]
        Earning = 0,
        [Display(Name = "Deducción")]
        Deduction = 1,
        [Display(Name = "Impuesto")]
        Tax = 2,
        [Display(Name = "Préstamo")]
        Loan = 3,
        [Display(Name = "Cooperativa")]
        Cooperative = 4,
        [Display(Name = "Contribución")]
        Contribution = 5,
        [Display(Name = "Horas extras")]
        ExtraHours = 6
    }

    public enum IndexBaseDeduction
    {
        [Display(Name = "Monto fijo")]
        FixedAmount = 4,
        [Display(Name = "Porcentaje de ganancia")]
        EarningPercent = 7,

    }

    public enum BatchEntity
    {
        [Display(Name = "Empleados")]
        Employee = 0,

        [Display(Name = "Dirección de empleados")]
        Employeeaddress = 1,

        [Display(Name = "Contacto de empleados")]
        EmployeeContactInfo = 2,

        [Display(Name = "Documentos de empleados")]
        EmployeeDocument = 3,

        [Display(Name = "Horas extras de empleados")]
        EmployeeExtraHours = 4,

        [Display(Name = "Ganancias de empleados")]
        EmployeeEarningCode = 5,

        [Display(Name = "Préstamos de empleados")]
        EmployeeLoans = 6,

        [Display(Name = "Bancos de empleados")]
        EmployeeBanks = 7,

        [Display(Name = "Impuestos de empleados")]
        EmployeeTax = 8,

        [Display(Name = "Deducciones de empleados")]
        EmployeeDeductionCode = 9,
        
        [Display(Name = "Horarios de empleados")]
        EmployeeWorkCalendars = 10,
        
        [Display(Name = "Control asistencia de empleados")]
        EmployeeWorkControlCalendar = 11,
    }

    public enum StatusWorkControl
    {
        [Display(Name = "Pendiente")]
        Pendint = 0,
        [Display(Name = "Pagado")]
        Paid = 1
    }
}
