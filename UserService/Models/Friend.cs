using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Models
{
    public class Friend
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int FriendUserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("FriendUserId")]
        public User FriendUser { get; set; }
    }
}
