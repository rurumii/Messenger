using MessageService.Data;
using Microsoft.AspNetCore.Mvc;
using MessageService.Models;

namespace MessageService.Controllers
{
    [ApiController]
    [Route("api/chats")]
    public class ChatController : ControllerBase
    {
        private readonly MessageDbContext _context;

        public ChatController (MessageDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public IActionResult CreateChat([FromBody] CreateChatDTO chat)
        {
            if (chat.User1Id == chat.User2Id)
            {
                return BadRequest(new { message = "User cannot chat with themselves" });
            }

            var existing = _context.Chats.FirstOrDefault(c =>
            (c.User1Id == chat.User1Id && c.User2Id == chat.User2Id) ||
            (c.User1Id == chat.User2Id && c.User2Id == chat.User1Id));

            if (existing != null)
            {
                return Conflict(new { message = "Chat already exists", chatId = existing.Id });
            }

            var newchat = new Chat
            {
                User1Id = chat.User1Id,
                User2Id = chat.User2Id,
                Name = chat.Name
            };

            _context.Chats.Add(newchat);
            _context.SaveChanges();

            return Ok(new 
            { 
                message = "Chat created successfully",
                chatId = newchat.Id
            });

        }

        [HttpGet("user/{userId}")]
        public IActionResult GetChatByUserId (int userId)
        {
            var chats = _context.Chats
                .Where(c=> c.User1Id == userId || c.User2Id == userId)
                .ToList();

            if (chats == null || chats.Count == 0)
            {
                return NotFound(new { message = "No chats found for this user" });
            }
            return Ok(chats);
        }
        [HttpGet("{chatId}")]
        public IActionResult GetChatById (int chatId)
        {
            var chat = _context.Chats.FirstOrDefault(c=>c.Id == chatId);

            if (chat == null)
            {
                return NotFound(new { message = "Chat not found" });
            }

            return Ok(chat);
        }
        [HttpDelete("{chatId}")]
        public IActionResult DeleteChat (int chatId)
        {
            var chat = _context.Chats.FirstOrDefault( c => c.Id == chatId);

            if (chat == null)
            {
                return NotFound(new { message = "Chat not found" });
            }

            _context.Chats.Remove(chat);
            _context.SaveChanges();

            return Ok(new { message = "Chat deleted successfully" });
        }
        [HttpPut("{chatId}")]
        public IActionResult UpdateChatName (int chatId, [FromBody] UpdateChatNameDTO dto)
        {
            var chat = _context.Chats.FirstOrDefault (c => c.Id == chatId);

            if (chat == null)
            {
                return NotFound(new { message = "Chat not found" });
            }
            chat.Name = dto.Name;
            _context.SaveChanges();
            return Ok(new {message = "Chat name updated successfully", chatId=chat.Id, newName=chat.Name});
        }
    }
}
