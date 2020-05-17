using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw3.DAL;
using Cw3.DTOs.Requests;
using Cw3.Models;
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
            var res = _dbservice.EnrollStudent(req);
            if(res.enrollment == null)
            {
                return BadRequest(res);
            }
            return Created("api/enrollments/" + res.enrollment.IdEnrollment, res);
        }
        [HttpGet("{id}")]
        public IActionResult getEnrollment(int id)
        {
            Enrollment e = _dbservice.GetEnrollment(id);
            if (e == null) return NotFound();
            return Ok(e);
        }
    }
}