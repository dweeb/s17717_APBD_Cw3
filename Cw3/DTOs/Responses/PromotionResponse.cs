using Cw3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DTOs.Responses
{
    public class PromotionResponse
    {
        public string responseText { get; set; }
        public Enrollment enrollment { get; set; }
    }
}
