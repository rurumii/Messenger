using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class UpdateUserDTO
    {
        public string Username { get; set; }
        public string UserTag { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Bio { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
