using Cw3.DTOs.Requests;
using Cw3.DTOs.Responses;
using Cw3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DAL
{
    public class SqlServerEnrollmentService : IDbEnrollmentService
    {
        private readonly string connectionString;
        public SqlServerEnrollmentService()
        {
            connectionString = "Data Source=db-mssql;Initial Catalog=s17717;Integrated Security=True";
        }
        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest req)
        {
            throw new NotImplementedException();
        }

        public Enrollment GetEnrollment(int id)
        {
            throw new NotImplementedException();
        }
    }
}
