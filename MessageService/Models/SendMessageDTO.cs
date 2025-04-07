namespace MessageService.Models
{
    public class SendMessageDTO
    {
        public int ChatId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId   { get; set; }
        public string Content { get; set; }
    }
}
