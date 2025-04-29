using System.ComponentModel.DataAnnotations;

namespace MessengerClient.Models
{
    public class RegistrationDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        public string? UserTag { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm your password.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
    }
}
