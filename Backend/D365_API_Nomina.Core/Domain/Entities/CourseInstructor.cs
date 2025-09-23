using D365_API_Nomina.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class CourseInstructor: AuditableCompanyEntity
    {
        /// <summary>
        /// Required / Max 20
        /// </summary>
        public string CourseId { get; set; }
        /// <summary>
        /// Required / Max 20
        /// </summary>
        public string InstructorId { get; set; }
        /// <summary>
        /// Max 300
        /// </summary>
        public string  Comment { get; set; }
    }
}
