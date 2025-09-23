using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Course
{
    public class CourseResponse
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseTypeId { get; set; }
        public string CourseTypeName { get; set; }

        public bool IsMatrixTraining { get; set; }
        public InternalExternal InternalExternal { get; set; }
        public string CourseParentId { get; set; }
        public string ClassRoomId { get; set; }
        public string ClassRoomName { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int MinStudents { get; set; }
        public int MaxStudents { get; set; }
        public int Periodicity { get; set; }
        public int QtySessions { get; set; }
        public string Description { get; set; }
        public string Objetives { get; set; }
        public string Topics { get; set; }

        public string URLDocuments { get; set; }
        public CourseStatus CourseStatus { get; set; }
    }
}
