using Microsoft.AspNetCore.Mvc;
using MessageService.Data;
using MessageService.Models;
namespace MessageService.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MessageDbContext _context;
        
        public MessageController(MessageDbContext context)
        {
            _context = context;
        }

        [HttpPost("send")]
        public IActionResult SendMessage([FromBody] SendMessageDTO message)
        {
            if (string.IsNullOrWhiteSpace(message.Content))
            {
                return BadRequest(new { message = "Message content cannot be empty" });
            }

            var newMessage = new Message
            {
                ChatId = message.ChatId,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                Content = message.Content,
                Timestamp = message.Timestamp,
            };
            _context.Messages.Add(newMessage);
            _context.SaveChanges();

            return Ok(new { message = "Message sent!", newMessageId = newMessage.Id });
        }

        [HttpGet("chat/{chatId}")]
        public IActionResult GetMessagesByChat(int chatId)
        {
            var messages = _context.Messages
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.Timestamp)
                .ToList();

            return Ok(messages);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMessage (int id)
        {
            var message = _context.Messages.Find(id);
            if (message == null)
            {
                return NotFound(new { message = "Message not found" });
            }

            _context.Messages.Remove(message);
            _context.SaveChanges();

            return Ok(new { message = "Message deleted" });
        }
    }
}
