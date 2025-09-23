using D365_API_Nomina.Core.Domain.Common;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class Course : AuditableCompanyEntity
    {
        /// <summary>
        /// Automatic
        /// </summary>
        public string CourseId { get; set; }
        /// <summary>
        /// Required / Max 50
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// Required / Max 20
        /// </summary>
        public string CourseTypeId { get; set; }
        public bool IsMatrixTraining { get; set; }
        /// <summary>
        /// Internal = 0, External = 1
        /// </summary>
        public InternalExternal InternalExternal { get; set; }
        /// <summary>
        /// Max 20
        /// </summary>
        public string CourseParentId { get; set; }
        /// <summary>
        /// Required / Max 20
        /// </summary>
        public string ClassRoomId { get; set; }
        /// <summary>
        /// Required
        /// </summary>
        public DateTime StartDateTime { get; set; }
        /// <summary>
        /// Required
        /// </summary>
        public DateTime EndDateTime { get; set; }
        /// <summary>
        /// Required
        /// </summary>
        public int MinStudents { get; set; }
        /// <summary>
        /// Required
        /// </summary>
        public int MaxStudents { get; set; }
        /// <summary>
        /// Required
        /// </summary>
        public int Periodicity { get; set; }
        /// <summary>
        /// Required
        /// </summary>
        public int QtySessions { get; set; }

        /// <summary>
        /// Max 300
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Required / Max 1000
        /// </summary>
        public string Objetives { get; set; }
        /// <summary>
        /// Required / Max 1000
        /// </summary>
        public string Topics { get; set; }

        /// <summary>
        /// Created = 0, InProcess = 1, Closed = 2
        /// </summary>
        public CourseStatus CourseStatus { get; set; }

        public string URLDocuments { get; set; }

    }
}
