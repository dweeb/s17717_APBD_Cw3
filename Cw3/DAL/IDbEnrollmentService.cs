﻿using Cw3.DTOs.Requests;
using Cw3.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DAL
{
    public interface IDbEnrollmentService
    {
        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest req);
    }
}
