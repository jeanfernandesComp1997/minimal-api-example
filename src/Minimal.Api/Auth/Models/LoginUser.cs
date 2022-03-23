using System.ComponentModel.DataAnnotations;

namespace Minimal.Api.Auth.Models
{
    public class LoginUser
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}