using D365_API_Nomina.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class Instructor: AuditableCompanyEntity
    {
        /// <summary>
        /// Automatic
        /// </summary>
        public string InstructorId { get; set; }
        /// <summary>
        /// Required, Max 50
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Max 20
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Max 100
        /// </summary>
        public string Mail { get; set; }
        /// <summary>
        /// Required, Max 100
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// Max 100
        /// </summary>
        public string Comment { get; set; }
    }
}
