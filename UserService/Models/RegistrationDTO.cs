using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class RegistrationDTO
    {
        [Required]
        public string Username { get; set; }
        public string? UserTag { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
