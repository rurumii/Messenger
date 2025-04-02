using System.ComponentModel.DataAnnotations;

namespace MessageService.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SenderId { get; set; }
        [Required]
        public int ReceiverId { get; set; }
        [Required]
        public int ChatId { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
