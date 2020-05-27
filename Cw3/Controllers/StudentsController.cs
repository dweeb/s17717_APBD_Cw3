using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw3.DAL;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Cw3.DTOs.Requests;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

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
            Student s = _dbservice.GetStudent(id);
            if (s == null) return NotFound();
            return Ok(s);
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
        [HttpPost("{id}/password")]
        [Authorize(Roles = "employee")]
        public IActionResult ChangePassword(ChangePasswordRequest req)
        {
            if (_dbservice.GetStudent(req.index) == null)
                return NotFound("Student with this index doesn't exist.");
            byte[] salt = new byte[128 / 8];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string hashedPw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: req.password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,       //  eyeballin' it
                numBytesRequested: 256 / 8
            ));
            string saltString = Convert.ToBase64String(salt);
            if (_dbservice.SetStudentPassword(req.index, req.password, saltString))
                return Ok();
            return BadRequest();
        }
    }
}