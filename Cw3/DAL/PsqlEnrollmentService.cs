using Cw3.DTOs.Requests;
using Cw3.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DAL
{
    public class PsqlEnrollmentService : IDbEnrollmentService
    {
        private readonly string connectionString;
        public PsqlEnrollmentService()
        {
            connectionString = System.IO.File.ReadLines("auth\\pg.cstr").First();
            // constring in gitignored file because security
            // "Username=;Password=;Host=;Port=;Database=;SSL Mode=Prefer";
        }
        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
