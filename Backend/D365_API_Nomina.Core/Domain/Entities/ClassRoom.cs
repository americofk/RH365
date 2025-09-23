using D365_API_Nomina.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class ClassRoom: AuditableCompanyEntity
    {
        /// <summary>
        /// Automatic
        /// </summary>
        public string ClassRoomId { get; set; }
        /// <summary>
        /// Required / Max 50
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Required / Max 50
        /// </summary>
        public string CourseLocationId { get; set; }
        /// <summary>
        /// Required
        /// </summary>
        public int MaxStudentQty { get; set; }
        /// <summary>
        /// Required
        /// </summary>
        public TimeSpan AvailableTimeStart { get; set; }
        ///// <summary>
        ///// Required
        ///// </summary>
        public TimeSpan AvailableTimeEnd { get; set; }
        /// <summary>
        /// Max 100
        /// </summary>
        public string Comment { get; set; }
    }
}
