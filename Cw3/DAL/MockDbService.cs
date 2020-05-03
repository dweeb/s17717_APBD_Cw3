using Cw3.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DAL
{
    public class MockDbService : IDbStudentService
    {
        private static IEnumerable<Student> _students;
        static MockDbService()
        {
            _students = new List<Student>
            {
                new Student {IdStudent = 1, FirstName = "Janusz", LastName = "Tracz"},
                new Student {IdStudent = 2, FirstName = "Janosik", LastName = "Tracz"},
                new Student {IdStudent = 3, FirstName = "Janusz", LastName = "Jankowski"}
            };
        }

        public IActionResult GetEnrollment(string id)
        {
            throw new NotImplementedException();
        }

        public IActionResult GetStudent(string id)
        {
            throw new NotImplementedException();
        }

        public Enrollment GetStudentEnrollment(string id)
        {
            throw new NotImplementedException();
        }

        Student IDbStudentService.GetStudent(string id)
        {
            throw new NotImplementedException();
        }

        List<Student> IDbStudentService.GetStudents()
        {
            throw new NotImplementedException();
        }
    }
}
