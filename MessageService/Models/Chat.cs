using System.ComponentModel.DataAnnotations;
namespace MessageService.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int User1Id { get; set; }
        [Required]
        public int User2Id { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
