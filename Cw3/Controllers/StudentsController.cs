using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw3.DAL;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbStudentService _dbservice;
        public StudentsController(IDbStudentService dbService)
        {
            _dbservice = dbService;
        }
        [HttpGet("{id}")]
        public IActionResult GetStudent(string id)
        {
            return Ok(_dbservice.GetStudent(id));
        }
        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok(_dbservice.GetStudents());
        }
        [HttpGet("{id}/enrollment")]
        public IActionResult GetStudentEnrollment(string id)
        {
            return Ok(_dbservice.GetStudentEnrollment(id));
        }
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }
        [HttpPut("{id}")]
        public IActionResult PutStudent(int id)
        {
            return Ok("Aktualizacja ukonczona");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            return Ok("Usuwanie ukonczone");
        }
    }
}