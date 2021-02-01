using System;
using DLL.DBContext;
using DLL.Models;

namespace DLL.Repositories
{
    public interface ICourseStudentRepository : IRepositoryBase<CourseStudent>
    {

    }

    public class CourseStudentRepository : RepositoryBase<CourseStudent>, ICourseStudentRepository
    {
        public CourseStudentRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
