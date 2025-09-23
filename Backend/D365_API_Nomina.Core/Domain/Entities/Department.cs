using D365_API_Nomina.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class Department: AuditableCompanyEntity
    {
        public string DepartmentId { get; set; }
        public string Name { get; set; }
        public int QtyWorkers { get; set; }

        //Foreign key for employee
        //public string ResponsibleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        public bool DepartamentStatus { get; set; } = true;

        //Compañia por la que va a guardar y buscar 
        //[MaxLength(5)]
        //public string InCompany { get; set; }

    }
}
