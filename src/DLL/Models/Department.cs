using System;
using DLL.Models.Interfaces;

namespace DLL.Models
{
    public class Department : ISoftDeletable
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
