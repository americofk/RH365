using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class Employee : AuditableCompanyEntity
{
   
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(20)]
    public string EmployeeCode { get; set; } = null!;

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    public string? PersonalTreatment { get; set; }

    public DateTime BirthDate { get; set; }

    public int Gender { get; set; }

    public int Age { get; set; }

    public int DependentsNumbers { get; set; }

    public int MaritalStatus { get; set; }

    [StringLength(20)]
    public string NSS { get; set; } = null!;

    [StringLength(20)]
    public string ARS { get; set; } = null!;

    [StringLength(20)]
    public string AFP { get; set; } = null!;

    public DateTime AdmissionDate { get; set; }

    public long CountryRecID { get; set; }

    public int EmployeeType { get; set; }

    public bool HomeOffice { get; set; }

    public bool OwnCar { get; set; }

    public bool HasDisability { get; set; }

    public TimeOnly WorkFrom { get; set; }

    public TimeOnly WorkTo { get; set; }

    public TimeOnly BreakWorkFrom { get; set; }

    public TimeOnly BreakWorkTo { get; set; }

    public bool EmployeeStatus { get; set; }

    public DateTime? EndWorkDate { get; set; }

    public int PayMethod { get; set; }

    public DateTime StartWorkDate { get; set; }

    public int WorkStatus { get; set; }

    public int EmployeeAction { get; set; }

    public long? DisabilityTypeRecID { get; set; }

    public long? EducationLevelRecID { get; set; }

    public long? OccupationRecID { get; set; }

    [StringLength(5)]
    public string? Nationality { get; set; }

    public long? LocationRecID { get; set; }

    public bool ApplyForOvertime { get; set; }

    public bool IsFixedWorkCalendar { get; set; }

    [InverseProperty("EmployeeRefRec")]
    public virtual ICollection<CourseEmployee> CourseEmployees { get; set; } = new List<CourseEmployee>();

    [InverseProperty("EmployeeRefRec")]
    public virtual ICollection<EmployeeBanckAccount> EmployeeBanckAccounts { get; set; } = new List<EmployeeBanckAccount>();

    [InverseProperty("EmployeeRefRec")]
    public virtual ICollection<EmployeeContactsInf> EmployeeContactsInfs { get; set; } = new List<EmployeeContactsInf>();

    [InverseProperty("EmployeeRefRec")]
    public virtual ICollection<EmployeeDeductionCode> EmployeeDeductionCodes { get; set; } = new List<EmployeeDeductionCode>();

    [InverseProperty("EmployeeRefRec")]
    public virtual ICollection<EmployeeDepartment> EmployeeDepartments { get; set; } = new List<EmployeeDepartment>();

    [InverseProperty("EmployeeRefRec")]
    public virtual ICollection<EmployeeDocument> EmployeeDocuments { get; set; } = new List<EmployeeDocument>();

    [InverseProperty("EmployeeRefRec")]
    public virtual ICollection<EmployeeEarningCode> EmployeeEarningCodes { get; set; } = new List<EmployeeEarningCode>();

    [InverseProperty("EmployeeRefRec")]
    public virtual ICollection<EmployeeExtraHour> EmployeeExtraHours { get; set; } = new List<EmployeeExtraHour>();

    [InverseProperty("EmployeeRefRec")]
    public virtual ICollection<EmployeeHistory> EmployeeHistories { get; set; } = new List<EmployeeHistory>();

    [InverseProperty("EmployeeRefRec")]
    public virtual ICollection<EmployeeImage> EmployeeImages { get; set; } = new List<EmployeeImage>();

    [InverseProperty("EmployeeRefRec")]
    public virtual ICollection<EmployeeLoanHistory> EmployeeLoanHistories { get; set; } = new List<EmployeeLoanHistory>();

    [InverseProperty("EmployeeRefRec")]
    public virtual ICollection<EmployeeLoan> EmployeeLoans { get; set; } = new List<EmployeeLoan>();

    [InverseProperty("EmployeeRefRec")]
    public virtual ICollection<EmployeePosition> EmployeePositions { get; set; } = new List<EmployeePosition>();

    [InverseProperty("EmployeeRefRec")]
    public virtual ICollection<EmployeeTaxis> EmployeeTaxes { get; set; } = new List<EmployeeTaxis>();

    [InverseProperty("EmployeeRefRec")]
    public virtual ICollection<EmployeeWorkCalendar> EmployeeWorkCalendars { get; set; } = new List<EmployeeWorkCalendar>();

    [InverseProperty("EmployeeRefRec")]
    public virtual ICollection<EmployeeWorkControlCalendar> EmployeeWorkControlCalendars { get; set; } = new List<EmployeeWorkControlCalendar>();

    [InverseProperty("EmployeeRefRec")]
    public virtual ICollection<EmployeesAddress> EmployeesAddresses { get; set; } = new List<EmployeesAddress>();

    [InverseProperty("EmployeeRefRec")]
    public virtual ICollection<PayrollProcessAction> PayrollProcessActions { get; set; } = new List<PayrollProcessAction>();

    [InverseProperty("EmployeeRefRec")]
    public virtual ICollection<PayrollProcessDetail> PayrollProcessDetails { get; set; } = new List<PayrollProcessDetail>();
}
