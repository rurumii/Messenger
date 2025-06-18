using Microsoft.AspNetCore.Mvc;
using MessageService.Data;
using MessageService.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MessageService.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MessageService.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MessageDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserServiceClient _userService;

        public MessageController(MessageDbContext context, IMapper mapper, UserServiceClient userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;

        }
        [Authorize]
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDTO message)
        {
            if (string.IsNullOrWhiteSpace(message.Content))
            {
                return BadRequest(new { message = "Message content cannot be empty" });
            }

            string token = Request.Headers.Authorization.ToString().Replace("Bearer", "");

            var sender = await _userService.GetUserByIdAsync(message.SenderId, token);
            var receiver = await _userService.GetUserByIdAsync(message.ReceiverId, token);

            if (sender == null || receiver == null)
            {
                return NotFound(new { message = "Sender or receiver not found" });
            }
            var newMessage = _mapper.Map<Message>(message);
            newMessage.Timestamp = DateTime.UtcNow;

            await _context.Messages.AddAsync(newMessage);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Message sent!",
                newMessageId = newMessage.Id,
                chatId = newMessage.ChatId,
                timestamp = newMessage.Timestamp
            });
        }

        [Authorize]
        [HttpGet("chat/{chatId}")]
        public async Task<IActionResult> GetMessagesByChat(int chatId)
        {
            var messages = await _context.Messages
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            var messagesDtos = new List<MessageDTO>();
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");
            foreach (var msg in messages)
            {
                var sender = await _userService.GetUserByIdAsync(msg.SenderId, token);
                var dto = _mapper.Map<MessageDTO>(msg);
                dto.SenderName = sender?.Username ?? $"User {msg.SenderId}";
                messagesDtos.Add(dto);
            }

            return Ok(messagesDtos);
        }
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound(new { message = "Message not found" });
            }

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (message.SenderId != currentUserId)
            {
                return Forbid();
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Message deleted" });
        }
        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateMessage(int id, [FromBody] UpdateMessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
            {
                return BadRequest(new { message = "Message content cannot be empty" });
            }
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound(new { message = "Message not found" });
            }

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (message.SenderId != currentUserId)
            {
                return Forbid();
            }

            message.Content = dto.Content;
            message.EditedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Message updated" });
        }



    }
}
