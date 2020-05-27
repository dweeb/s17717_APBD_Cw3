using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DTOs.Requests
{
    public class ChangePasswordRequest
    {
        public string index { get; set; }
        public string password { get; set; }
    }
}
