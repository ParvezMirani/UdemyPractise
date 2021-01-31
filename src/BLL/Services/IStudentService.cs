using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DLL.Models;
using DLL.Repositories;
using Utility.Exceptions;

namespace BLL.Services
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllAsync();
        Task<Student> GetAAsync(string email);
        Task<Student> InsertAsync(Student student);
        Task<Student> UpdateAsync(string email, Student student);
        Task<Student> DeleteAsync(string email);
    }


    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }


        public async Task<Student> GetAAsync(string email)
        {
            var student = await _studentRepository.FindSingleAsync(x=>x.Email==email);

            if(student == null)
            {
                throw new ApplicationValidationException("Student not found");
            }

            return student;
        }


        public async Task<List<Student>> GetAllAsync()
        {
            return await _studentRepository.GetListAsync();
        }


        public async Task<Student> InsertAsync(Student student)
        {
            var studentExists = await _studentRepository.FindSingleAsync(x => x.Email == student.Email);

            if(studentExists!= null)
            {
                throw new ApplicationValidationException($"Student with {student.Email} already exist");
            }

            await _studentRepository.CreateAsync(student);

            if (await _studentRepository.SaveCompletedAsync())
            {
                return student;
            }
            throw new ApplicationValidationException("Problem occured CREATING student");

        }


        public async Task<Student> UpdateAsync(string email, Student student)
        {
            var dbStudent = await _studentRepository.FindSingleAsync(x => x.Email == email);

            if (dbStudent == null)
            {
                throw new ApplicationValidationException("Student Not found");
            }

            dbStudent.Name = student.Name;

            _studentRepository.Update(dbStudent);

            if (await _studentRepository.SaveCompletedAsync())
            {
                return dbStudent;
            }
            throw new ApplicationValidationException("Problem occured UPDATING student");
        }


        public async Task<Student> DeleteAsync(string email)
        {
            var student = await _studentRepository.FindSingleAsync(x => x.Email == email);

            if (student == null)
            {
                throw new ApplicationValidationException("Student Not fountd");
            }

            _studentRepository.Delete(student);

            if (await _studentRepository.SaveCompletedAsync())
            {
                return student;
            }
            throw new ApplicationValidationException("Problem occured DELETING student");
        }
    }
}
