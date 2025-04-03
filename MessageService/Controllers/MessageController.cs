using Microsoft.AspNetCore.Mvc;
using MessageService.Data;
using MessageService.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace MessageService.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MessageDbContext _context;
        private readonly IMapper _mapper;
        
        public MessageController(MessageDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDTO message)
        {
            if (string.IsNullOrWhiteSpace(message.Content))
            {
                return BadRequest(new { message = "Message content cannot be empty" });
            }

            var newMessage = _mapper.Map<Message>(message);
            newMessage.Timestamp = DateTime.UtcNow;
            await _context.Messages.AddAsync(newMessage);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Message sent!", newMessageId = newMessage.Id });
        }

        [HttpGet("chat/{chatId}")]
        public async Task<IActionResult> GetMessagesByChat(int chatId)
        {
            var messages = await _context.Messages
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            return Ok(messages);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage (int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound(new { message = "Message not found" });
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Message deleted" });
        }
    }
}
