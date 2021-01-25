using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok("Get All students");
        }

        [HttpGet("{code}")]
        public IActionResult GetA(string code)
        {
            return Ok("get this " + code + "department data");
        }

        [HttpPost]
        public IActionResult Insert()
        {
            return Ok("inserted");
        }

        [HttpPut("{code}")]
        public IActionResult UpdateADepartment(string code)
        {
            return Ok($"updated this {code} is updated");
        }

        [HttpDelete("{code}")]
        public IActionResult DeleteADepartment(string code)
        {
            return Ok($"deleted {code}");
        }
    }
}