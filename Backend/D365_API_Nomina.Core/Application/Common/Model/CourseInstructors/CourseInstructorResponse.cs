using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.CourseInstructors
{
    public class CourseInstructorResponse
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public string InstructorId { get; set; }
        public string Comment { get; set; }
        public string InstructorName { get; set; }
    }
}
