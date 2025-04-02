using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace UserService.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Display(Description = "Optional username tag shown in chats")]
        public string? UserTag { get; set; } // unique

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string? ProfileImageUrl { get; set; }

        [Display(Description = "Optional user bio shown in profile")]
        public string? Bio {  get; set; }
    }
}
