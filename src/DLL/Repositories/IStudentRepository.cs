using System.Collections.Generic;
using System.Threading.Tasks;
using DLL.DBContext;
using DLL.Models;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repositories
{

    public interface IStudentRepository
    {
        Task<Student> InsertAsync(Student student);
        Task<List<Student>> GetAllAsync();
        Task<Student> GetAAsync(string code);
        Task<Student> DeleteAsync(string code);
        Task<Student> UpdateAsync(string code, Student student);
    }



    public class StudentRepository:IStudentRepository
    {
        private readonly ApplicationDbContext _context;


        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Student> InsertAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return student;
        }


        public async Task<List<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }


        public async Task<Student> GetAAsync(string code)
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.Email == code);            
            return student;
        }


        public async Task<Student> DeleteAsync(string code)
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.Email == code);

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return student;
        }


        public async Task<Student> UpdateAsync(string code, Student student)
        {
            var studentToUpdate = await _context.Students.FirstOrDefaultAsync(x => x.Email == code);

            studentToUpdate.Name = student.Name;
            _context.Students.Update(studentToUpdate);
            await _context.SaveChangesAsync();
            return studentToUpdate;
        }
    }
}
