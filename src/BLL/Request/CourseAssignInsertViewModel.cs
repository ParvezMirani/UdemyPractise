using System;
using System.Threading;
using System.Threading.Tasks;
using BLL.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Request
{
    public class CourseAssignInsertViewModel
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }

    public class CourseAssignInsertViewModelValidatior: AbstractValidator<CourseAssignInsertViewModel>
    {
        private readonly IServiceProvider _serviceProvider;


        public CourseAssignInsertViewModelValidatior(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RuleFor(x => x.StudentId)
                .NotNull()
                .MustAsync(StudentIdExists)
                .WithMessage("Student Id Not found");
            RuleFor(x => x.CourseId)
                .NotNull()
                .MustAsync(CourseIdExists)
                .WithMessage("Course Id Not found");

        }

        private async Task<bool> CourseIdExists(int courseId, CancellationToken arg2)
        {
            var requiredSevice = _serviceProvider.GetRequiredService<ICourseService>();

            return ! await requiredSevice.IsIdExists(courseId);

        }

        private async Task<bool> StudentIdExists(int studentId, CancellationToken arg2)
        {
            var requiredSevice = _serviceProvider.GetRequiredService<IStudentService>();

            return !await requiredSevice.IsIdExists(studentId);
        }
    }
}
