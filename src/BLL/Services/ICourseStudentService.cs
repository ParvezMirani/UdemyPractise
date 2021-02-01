using System;
using System.Threading.Tasks;
using BLL.Request;
using DLL.Models;
using DLL.Repositories;
using DLL.ResponseViewModel;
using Utility.Exceptions;
using Utility.Models;

namespace BLL.Services
{
    public interface ICourseStudentService
    {
        Task<ApiSuccessResponse> InsertAsync(CourseAssignInsertViewModel request);
        Task<StudentCourseViewModel> CourseListAsync(int studentId);
    }

    public class CourseStudentService : ICourseStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseStudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<ApiSuccessResponse> InsertAsync(CourseAssignInsertViewModel request)
        {
            var isStudentAlreadyEnrolled = await _unitOfWork.CourseStudentRepository.FindSingleAsync(
                x => x.CourseId == request.CourseId && x.StudentId == request.StudentId);

            if (isStudentAlreadyEnrolled != null)
            {
                throw new ApplicationValidationException("This student is already enrolled in this course");
            }

            var courseStudent = new CourseStudent()
            {
                CourseId = request.CourseId,
                StudentId = request.StudentId
            };

            await _unitOfWork.CourseStudentRepository.CreateAsync(courseStudent);

            if (await _unitOfWork.SaveCompletedAsync())
            {
                return new ApiSuccessResponse()
                {
                    Message = $"Student enrolled in {courseStudent.CourseId}",
                    StatusCode = 200
                };
            }

            throw new ApplicationValidationException("Something went wrong while enrolling Student");
        }


        public Task<StudentCourseViewModel> CourseListAsync(int studentId)
        {
            return _unitOfWork.StudentRepository.GetSpecificStudentCourseListAsync(studentId);
        }

    }
}
