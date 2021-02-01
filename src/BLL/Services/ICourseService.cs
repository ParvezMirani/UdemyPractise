using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Request;
using DLL.Models;
using DLL.Repositories;
using Utility.Exceptions;

namespace BLL.Services
{
    public interface ICourseService
    {
        Task<List<Course>> GetAllAsync();
        Task<Course> GetAAsync(string code);
        Task<Course> InsertAsync(CourseInsertRequestViewModel request);
        Task<Course> UpdateAsync(string code, Course course);
        Task<Course> DeleteAsync(string code);

        Task<bool> IsCodeExists(string code);
        Task<bool> IsNameExists(string name);
        Task<bool> IsIdExists(int id);
    }


    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<List<Course>> GetAllAsync()
        {
            return await _unitOfWork.CourseRepository.GetListAsync();
        }


        public async Task<Course> GetAAsync(string code)
        {
            var course = await _unitOfWork.CourseRepository.FindSingleAsync(x => x.Code == code);

            if (course == null)
            {
                throw new ApplicationValidationException("Course not found");
            }

            return course;
        }


        public async Task<Course> InsertAsync(CourseInsertRequestViewModel request)
        {
            Course acourse = new Course();
            acourse.Code = request.Code;
            acourse.Name = request.Name;
            acourse.Credit = request.Credit;
            await _unitOfWork.CourseRepository.CreateAsync(acourse);

            if (await _unitOfWork.SaveCompletedAsync())
            {
                return acourse;
            }

            throw new ApplicationValidationException("Adding new course failed");
        }


        public async Task<Course> UpdateAsync(string code, Course updateCourse)
        {
            var course = await _unitOfWork.CourseRepository.FindSingleAsync(x => x.Code == code);

            if (course == null)
            {
                throw new ApplicationValidationException("Course not found");
            }

            if (!string.IsNullOrWhiteSpace(updateCourse.Code))
            {
                var existAlreadyCode = await _unitOfWork.CourseRepository.FindSingleAsync(x => x.Code == updateCourse.Code);
                if (existAlreadyCode != null)
                {
                    throw new ApplicationValidationException($"Code : '{updateCourse.Code}' already exists in our system");
                }

                course.Code = updateCourse.Code;
            }

            if (!string.IsNullOrWhiteSpace(updateCourse.Name))
            {
                var existAlreadyName = await _unitOfWork.CourseRepository.FindSingleAsync(x => x.Name == updateCourse.Name);
                if (existAlreadyName != null)
                {
                    throw new ApplicationValidationException($"Name : '{updateCourse.Name}' already exists in our system");
                }

                course.Name = updateCourse.Name;
            }

            if (updateCourse.Credit>0)
            {
                course.Credit = updateCourse.Credit;
            }

            _unitOfWork.CourseRepository.Update(course);

            if (await _unitOfWork.SaveCompletedAsync())
            {
                return course;
            }

            throw new ApplicationValidationException("Problem Occured updating course");
        }


        public async Task<Course> DeleteAsync(string code)
        {
            var course = await _unitOfWork.CourseRepository.FindSingleAsync(x => x.Code == code);

            if (course == null)
            {
                throw new ApplicationValidationException("Course not found");
            }

            _unitOfWork.CourseRepository.Delete(course);

            if (await _unitOfWork.SaveCompletedAsync())
            {
                return course;
            }

            throw new ApplicationValidationException("Problem occured while deleting Course");
        }



        public async Task<bool> IsCodeExists(string code)
        {
            var course = await _unitOfWork.CourseRepository.FindSingleAsync(x => x.Code == code);

            if (course == null)
            {
                return true;
            }

            return false;
        }


        public async Task<bool> IsNameExists(string name)
        {
            var course = await _unitOfWork.CourseRepository.FindSingleAsync(x => x.Name == name);

            if (course == null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsIdExists(int id)
        {
            var course = await _unitOfWork.CourseRepository.FindSingleAsync(x => x.CourseId == id);

            if (course == null)
            {
                return true;
            }

            return false;
        }
    }
}
