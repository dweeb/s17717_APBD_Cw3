using Cw3.Models;
using System.Collections.Generic;

namespace Cw3.DAL
{
    public interface IDbStudentService
    {
        public List<Student> GetStudents();
        public Student GetStudent(string id);
        public Enrollment GetStudentEnrollment(string id);
        public bool SetStudentPassword(string id, string password, string salt);
    }
}
