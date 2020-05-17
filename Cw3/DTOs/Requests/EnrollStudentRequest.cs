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
        public DateTime Birthdate { get; set; }
        // right now only accepts dates in the format "1999-03-30T00:00:00"
        /*
        public void SetBirthDate(String date)
        {
            Birthdate = DateTime.Parse(date);
        }
        */
        [Required]
        public string Studies { get; set; }
    }
}
