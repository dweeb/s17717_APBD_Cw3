using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw3.DAL;
using Cw3.DTOs.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IDbEnrollmentService _dbservice;
        public EnrollmentsController(IDbEnrollmentService dbService)
        {
            _dbservice = dbService;
        }
        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest req)
        {
            Console.WriteLine(req.FirstName);
            Console.WriteLine(req.Birthdate);
            return Ok();
        }
    }
}