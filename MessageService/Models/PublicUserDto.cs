namespace MessageService.Models
{
    public class PublicUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string UserTag { get; set; }
        public string Email { get; set; }
        public string Bio {  get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
