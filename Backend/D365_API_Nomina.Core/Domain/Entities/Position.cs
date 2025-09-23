using D365_API_Nomina.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    /// <summary>
    /// Puesto
    /// </summary>
    public class Position: AuditableCompanyEntity
    {
        /// <summary>
        /// Automatico
        /// </summary>
        public string PositionId { get; set; }
        /// <summary>
        /// Reqhuired / Max 50
        /// </summary>
        public string PositionName { get; set; }

        public bool IsVacant { get; set; } = false;

        /// <summary>
        /// Required / Max 20
        /// </summary>
        //Foreign key for department
        public string DepartmentId { get; set; }
        /// <summary>
        /// Required / Max 20
        /// </summary>
        //Foreign key for job - Cargo
        public string JobId { get; set; }

        /// <summary>
        /// Max 20
        /// </summary>
        //Foreign key for job
        public string NotifyPositionId { get; set; }

        public bool PositionStatus { get; set; } = true;
        /// <summary>
        /// Required
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Required
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Max 200
        /// </summary>
        public string Description { get; set; }
    }
}
