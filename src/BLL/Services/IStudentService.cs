using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Request;
using DLL.Models;
using DLL.Repositories;
using Utility.Exceptions;

namespace BLL.Services
{
    public interface IStudentService
    {
        IQueryable<Student> GetAll();
        Task<Student> GetAAsync(string email);
        Task<Student> InsertAsync(StudentInsertRequestViewModel student);
        Task<Student> UpdateAsync(string email, Student student);
        Task<Student> DeleteAsync(string email);
        Task<bool> IsEmailExists(string email);
        Task<bool> IsNameExists(string name);
        Task<bool> IsIdExists(int studentId);
    }


    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<Student> GetAAsync(string email)
        {
            var student = await _unitOfWork.StudentRepository.FindSingleAsync(x=>x.Email==email);

            if(student == null)
            {
                throw new ApplicationValidationException("Student not found");
            }

            return student;
        }


        public IQueryable<Student> GetAll()
        {
            return _unitOfWork.StudentRepository.QueryAll();
        }


        public async Task<Student> InsertAsync(StudentInsertRequestViewModel studentInsertRequest)
        {
            var newStudent = new Student()
            {
                Email = studentInsertRequest.Email,
                Name = studentInsertRequest.Name,
                DepartmentId = studentInsertRequest.DepartmentId
            };

            await _unitOfWork.StudentRepository.CreateAsync(newStudent);

            if (await _unitOfWork.SaveCompletedAsync())
            {
                return newStudent;
            }
            throw new ApplicationValidationException("Problem occured CREATING student");

        }


        public async Task<Student> UpdateAsync(string email, Student student)
        {
            var dbStudent = await _unitOfWork.StudentRepository.FindSingleAsync(x => x.Email == email);

            if (dbStudent == null)
            {
                throw new ApplicationValidationException("Student Not found");
            }

            dbStudent.Name = student.Name;

            _unitOfWork.StudentRepository.Update(dbStudent);

            if (await _unitOfWork.SaveCompletedAsync())
            {
                return dbStudent;
            }
            throw new ApplicationValidationException("Problem occured UPDATING student");
        }


        public async Task<Student> DeleteAsync(string email)
        {
            var student = await _unitOfWork.StudentRepository.FindSingleAsync(x => x.Email == email);

            if (student == null)
            {
                throw new ApplicationValidationException("Student Not fountd");
            }

            _unitOfWork.StudentRepository.Delete(student);

            if (await _unitOfWork.SaveCompletedAsync())
            {
                return student;
            }
            throw new ApplicationValidationException("Problem occured DELETING student");
        }

        public async Task<bool> IsEmailExists(string email)
        {
            var student = await _unitOfWork.StudentRepository.FindSingleAsync(x => x.Email == email);

            if(student == null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsNameExists(string name)
        {
            var student = await _unitOfWork.StudentRepository.FindSingleAsync(x => x.Name == name);

            if (student == null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsIdExists(int studentId)
        {
            var student = await _unitOfWork.StudentRepository.FindSingleAsync(x => x.StudentId == studentId);

            if (student == null)
            {
                return true;
            }

            return false;
        }
    }
}
