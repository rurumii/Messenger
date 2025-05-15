namespace MessageService.Models
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; } 
        public string SenderName { get; set; }
        public string Content { get; set; } = "";
        public DateTime SentAt { get; set; }
    }
}
