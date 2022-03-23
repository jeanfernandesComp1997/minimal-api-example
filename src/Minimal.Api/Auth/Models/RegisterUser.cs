using System.ComponentModel.DataAnnotations;

namespace Minimal.Api.Auth.Models
{
    public class RegisterUser
    {
        [Required]
        public string Email { get; private set; }
        [Required]
        public string Password { get; private set; }
        [Required]
        public string ConfirmPassword { get; private set; }

        public RegisterUser(string email, string password, string confirmPassword)
        {
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
    }
}