using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Reports
{
    public class DGT4Response
    {
        public string ResgisterType { get; set; } = "E";
        public string Process { get; set; } = "T4";
        public string RNC { get; set; }
        public string Period { get; set; }

        public List<DGT4Detail> Details { get; set; }

        public string ResgisterTypeSummary { get; set; } = "S";
        public string RegisterQty { get; set; }
    }

    public class DGT4Detail
    {
        public string NoveltyType { get; set; }
        public string EmployeeName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public string Salary { get; set; }
        public string AdmissionDate { get; set; }
        public string DocumentNumber { get; set; }
    }
    
    //public class DGT4Details
    //{
    //    public string ResgisterType { get; set; }
    //    public string ActionType { get; set; }
    //    public string DocumentType { get; set; }
    //    public string DocumentNumber { get; set; }
    //    public string Names { get; set; }
    //    public string LastName { get; set; }
    //    public string SecondLastName { get; set; }
    //    public string BirthDate { get; set; }
    //    public string Gender { get; set; }
    //    public string Salary { get; set; }
    //    public string StartDate { get; set; }
    //    public string EndDate { get; set; }
    //    public string Job { get; set; }
    //    public string JobDescription { get; set; }
    //    public string StartVacation { get; set; }
    //    public string EndVacation { get; set; }
    //    public string Turn { get; set; }
    //    public string Location { get; set; }
    //    public string Nationality { get; set; }
    //    public string ChangeDate { get; set; }
    //    public string EductionalLevel { get; set; }
    //    public string Disability { get; set; }
    //}
}
