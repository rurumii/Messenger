namespace MessengerClient.Models
{
    public class ChatDTO
    {
        public int Id { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        //public string? Name { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
