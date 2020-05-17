using System;
using System.ComponentModel.DataAnnotations;

namespace Cw3.DTOs.Requests
{
    public class EnrollStudentRequest
    {
        [Required]
        public string IndexNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public String Birthdate { get; set; }
        // using string is much better than DateTime since postgres is really robust when it comes to date formats
        [Required]
        public string Studies { get; set; }
    }
}
