using System.ComponentModel.DataAnnotations;

namespace Techorda.Auth.Models.Account
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1, 100)]
        public int Age { get; set; }
    }
}
