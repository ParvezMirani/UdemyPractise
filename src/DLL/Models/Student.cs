using System;
using System.Collections.Generic;
using DLL.Models.Interfaces;

namespace DLL.Models
{
    public class Student : ISoftDeletable, ITrackable
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }

        public Department Department { get; set; }
        public ICollection<CourseStudent> CourseStudents {get; set;}
    }
}
