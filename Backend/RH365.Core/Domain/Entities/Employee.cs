using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class Employee
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public string EmployeeCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? PersonalTreatment { get; set; }

    public DateTime BirthDate { get; set; }

    public int Gender { get; set; }

    public int Age { get; set; }

    public int DependentsNumbers { get; set; }

    public int MaritalStatus { get; set; }

    public string Nss { get; set; } = null!;

    public string Ars { get; set; } = null!;

    public string Afp { get; set; } = null!;

    public DateTime AdmissionDate { get; set; }

    public long CountryRecId { get; set; }

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

    public long? DisabilityTypeRecId { get; set; }

    public long? EducationLevelRecId { get; set; }

    public long? OccupationRecId { get; set; }

    public string? Nationality { get; set; }

    public long? LocationRecId { get; set; }

    public bool ApplyForOvertime { get; set; }

    public bool IsFixedWorkCalendar { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<CourseEmployee> CourseEmployees { get; set; } = new List<CourseEmployee>();

    public virtual ICollection<EmployeeBankAccount> EmployeeBankAccounts { get; set; } = new List<EmployeeBankAccount>();

    public virtual ICollection<EmployeeContactsInf> EmployeeContactsInfs { get; set; } = new List<EmployeeContactsInf>();

    public virtual ICollection<EmployeeDeductionCode> EmployeeDeductionCodes { get; set; } = new List<EmployeeDeductionCode>();

    public virtual ICollection<EmployeeDepartment> EmployeeDepartments { get; set; } = new List<EmployeeDepartment>();

    public virtual ICollection<EmployeeDocument> EmployeeDocuments { get; set; } = new List<EmployeeDocument>();

    public virtual ICollection<EmployeeEarningCode> EmployeeEarningCodes { get; set; } = new List<EmployeeEarningCode>();

    public virtual ICollection<EmployeeExtraHour> EmployeeExtraHours { get; set; } = new List<EmployeeExtraHour>();

    public virtual ICollection<EmployeeHistory> EmployeeHistories { get; set; } = new List<EmployeeHistory>();

    public virtual ICollection<EmployeeImage> EmployeeImages { get; set; } = new List<EmployeeImage>();

    public virtual ICollection<EmployeeLoanHistory> EmployeeLoanHistories { get; set; } = new List<EmployeeLoanHistory>();

    public virtual ICollection<EmployeeLoan> EmployeeLoans { get; set; } = new List<EmployeeLoan>();

    public virtual ICollection<EmployeePosition> EmployeePositions { get; set; } = new List<EmployeePosition>();

    public virtual ICollection<EmployeeTaxis> EmployeeTaxes { get; set; } = new List<EmployeeTaxis>();

    public virtual ICollection<EmployeeWorkCalendar> EmployeeWorkCalendars { get; set; } = new List<EmployeeWorkCalendar>();

    public virtual ICollection<EmployeeWorkControlCalendar> EmployeeWorkControlCalendars { get; set; } = new List<EmployeeWorkControlCalendar>();

    public virtual ICollection<EmployeesAddress> EmployeesAddresses { get; set; } = new List<EmployeesAddress>();

    public virtual ICollection<PayrollProcessAction> PayrollProcessActions { get; set; } = new List<PayrollProcessAction>();

    public virtual ICollection<PayrollProcessDetail> PayrollProcessDetails { get; set; } = new List<PayrollProcessDetail>();
}
