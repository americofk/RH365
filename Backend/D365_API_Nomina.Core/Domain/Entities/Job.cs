using D365_API_Nomina.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    /// <summary>
    /// Cargo
    /// </summary>
    public class Job: AuditableCompanyEntity
    {
        /// <summary>
        /// Automatico
        /// </summary>
        public string JobId { get; set; }
        /// <summary>
        /// Required / Max 50
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Max 200
        /// </summary>
        public string Description { get; set; }
        public bool JobStatus { get; set; } = true;

    }
}
