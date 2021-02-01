using System.Threading.Tasks;
using BLL.Request;
using BLL.Services;
using DLL.Models;
using LightQuery.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class StudentController : MainApiController
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [AsyncLightQuery(forcePagination: true, defaultPageSize: 10, defaultSort: "studentId desc")]
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(_studentService.GetAll());
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetA(string email)
        {
            return Ok(await _studentService.GetAAsync(email));
        }

        [HttpPost]
        public async Task<IActionResult> Insert(StudentInsertRequestViewModel studentInsertRequest)
        {
            return Ok(await _studentService.InsertAsync(studentInsertRequest));
        }

        [HttpPut("{email}")]
        public async Task<IActionResult> Update(string email,Student student)
        {
            return Ok(await _studentService.UpdateAsync(email,student));
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> Delete(string email)
        {
            return Ok(await _studentService.DeleteAsync(email));
        }
    }
}