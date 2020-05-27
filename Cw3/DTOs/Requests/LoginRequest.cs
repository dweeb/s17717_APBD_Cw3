using System.ComponentModel.DataAnnotations;

namespace Cw3.DTOs.Requests
{
    public class LoginRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}