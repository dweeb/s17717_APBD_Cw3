using Cw3.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DAL
{
    public interface IDbService
    {
        // Student
        public List<Student> GetStudents();
        public Student GetStudent(string id);
        public Enrollment GetEnrollment(string id);
    }
}
