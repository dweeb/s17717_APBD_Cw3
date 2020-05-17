using System;
using System.ComponentModel.DataAnnotations;

namespace Cw3.DTOs.Requests
{
    public class PromotionRequest
    {
        [Required]
        public String Studies { get; set; }
        [Required]
        public int Semester { get; set; }
    }
}
