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
        private readonly IDbService _dbservice;
        public StudentsController(IDbService dbService)
        {
            _dbservice = dbService;
        }
        [HttpGet("{id}")]
        public IActionResult GetStudent(string id)
        {
            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s17717;Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "select s.FirstName, s.LastName, s.BirthDate, u.Name, e.Semester from Student as s " +
                        "join Enrollment e on s.IdEnrollment = e.IdEnrollment join Studies u on e.IdStudy = u.IdStudy " +
                        "where IndexNumber = @id;";
                command.Parameters.AddWithValue("id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                reader.Read();
                var s = new Student();
                s.FirstName = reader["FirstName"].ToString();
                s.LastName = reader["LastName"].ToString();
                s.BirthDate = reader["BirthDate"].ToString();
                s.StudiesName = reader["Name"].ToString();
                s.Semester = (int)reader["Semester"];
                return Ok(s);
                // copypasted code, I'm sure I'll get around to optimize that later
            }
        }
        [HttpGet]
        public IActionResult GetStudents()
        {
            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s17717;Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "select s.FirstName, s.LastName, s.BirthDate, u.Name, e.Semester from Student as s " +
                        "join Enrollment e on s.IdEnrollment = e.IdEnrollment join Studies u on e.IdStudy = u.IdStudy;";
                connection.Open();
                var reader = command.ExecuteReader();
                List<Student> returnList = new List<Student>();
                while (reader.Read())
                {
                    var s = new Student();
                    s.FirstName = reader["FirstName"].ToString();
                    s.LastName = reader["LastName"].ToString();
                    s.BirthDate = reader["BirthDate"].ToString();
                    s.StudiesName = reader["Name"].ToString();
                    s.Semester = (int) reader["Semester"];
                    returnList.Add(s);
                }
                return Ok(returnList);
            }
            return Ok(_dbservice.GetStudents());
        }
        [HttpGet("{id}/enrollment")]
        public IActionResult GetEnrollment(string id)
        {
            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s17717;Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText =   "select e.IdEnrollment, e.Semester, e.IdStudy, e.StartDate from Student as s " +
                                        "join Enrollment e on s.IdEnrollment = e.IdEnrollment " +
                                        //"where s.IndexNumber = " + id + ";";
                                        "where s.IndexNumber = @id;";
                command.Parameters.AddWithValue("id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                // currently only *:1 relation is possible between Student and Enrollment but not sure if that's the correct busines logic
                reader.Read();
                var enrollment = new Enrollment();
                enrollment.IdEnrollment = (int)reader["IdEnrollment"];
                enrollment.Semester = (int)reader["Semester"];
                enrollment.IdStudy = (int)reader["IdStudy"];
                enrollment.StartDate = reader["StartDate"].ToString();
                // could've used a proper constructor to 1-line it but it's whatever
                return Ok(enrollment);
            }
            return Ok();
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