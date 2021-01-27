using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class StudentController : MainApiController
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(StudentStatic.GetAllStudent());
        }
        
        [HttpGet("{email}")]
        public IActionResult GetA(string email)
        {
            return Ok(StudentStatic.GetAStudent(email));
        }

        [HttpPost]
        public IActionResult Insert([FromForm] Student Student)
        {
            return Ok(StudentStatic.InsertStudent(Student));
        }

        [HttpPut("{email}")]
        public IActionResult Update(string email, Student Student)
        {
            return Ok(StudentStatic.UpdateStudent(email, Student));
        }

        [HttpDelete("{email}")]
        public IActionResult DeleteAStudent(string email)
        {
            return Ok(StudentStatic.DeleteStudent(email));
        }
    }

    public static class StudentStatic
    {
        public static List<Student> AllStudent { get; set; } = new List<Student>();

        public static Student InsertStudent(Student Student)
        {
            AllStudent.Add(Student);
            return Student;
        }

        public static List<Student> GetAllStudent()
        {
            return AllStudent;
        }

        public static Student GetAStudent(string email)
        {
            return AllStudent.FirstOrDefault(x => x.Email == email);
        }

        public static Student UpdateStudent(string email, Student Student)
        {
            Student result = new Student();
            foreach (var aStudent in AllStudent)
            {
                if (email == aStudent.Email)
                {
                    aStudent.Name = Student.Name;
                    result = aStudent;
                }
            }
            return result;
        }

        public static Student DeleteStudent(string email)
        {
            var Student = AllStudent.FirstOrDefault(x => x.Email == email);
            AllStudent = AllStudent.Where(x => x.Email != Student.Email).ToList();
            return Student;
        }
    }
}