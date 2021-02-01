using System;
using System.Collections.Generic;
using DLL.Models;

namespace DLL.ResponseViewModel
{
    public class StudentCourseViewModel
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}
