using MessageService.Data;
using Microsoft.AspNetCore.Mvc;
using MessageService.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MessageService.Mapping;

namespace MessageService.Controllers
{
    [ApiController]
    [Route("api/chats")]
    public class ChatController : ControllerBase
    {
        private readonly MessageDbContext _context;
        private readonly IMapper _mapper;

        public ChatController (MessageDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("create")]    
        public async Task<IActionResult> CreateChat([FromBody] CreateChatDTO chat)
        {
            if (chat.User1Id == chat.User2Id)
            {
                return BadRequest(new { message = "User cannot chat with themselves" });
            }

            var existing = await _context.Chats.FirstOrDefaultAsync(c =>
            (c.User1Id == chat.User1Id && c.User2Id == chat.User2Id) ||
            (c.User1Id == chat.User2Id && c.User2Id == chat.User1Id));

            if (existing != null)
            {
                return Conflict(new { message = "Chat already exists", chatId = existing.Id });
            }

            var newchat = _mapper.Map<Chat>(chat);

            await _context.Chats.AddAsync(newchat);
            await _context.SaveChangesAsync();

            return Ok(new 
            { 
                message = "Chat created successfully",
                chatId = newchat.Id
            });

        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetChatByUserId (int userId)
        {
            var chats = await _context.Chats
                .Where(c=> c.User1Id == userId || c.User2Id == userId)
                .ToListAsync();

            if (chats == null || chats.Count == 0)
            {
                return NotFound(new { message = "No chats found for this user" });
            }
            return Ok(chats);
        }
        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetChatById (int chatId)
        {
            var chat = await _context.Chats.FirstOrDefaultAsync(c=>c.Id == chatId);

            if (chat == null)
            {
                return NotFound(new { message = "Chat not found" });
            }

            return Ok(chat);
        }
        [HttpDelete("{chatId}")]
        public async Task<IActionResult> DeleteChat (int chatId)
        {
            var chat = await _context.Chats.FirstOrDefaultAsync( c => c.Id == chatId);

            if (chat == null)
            {
                return NotFound(new { message = "Chat not found" });
            }

            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Chat deleted successfully" });
        }
        [HttpPut("{chatId}")]
        public async Task<IActionResult> UpdateChatName (int chatId, [FromBody] UpdateChatNameDTO dto)
        {
            var chat = await _context.Chats.FirstOrDefaultAsync (c => c.Id == chatId);

            if (chat == null)
            {
                return NotFound(new { message = "Chat not found" });
            }

            _mapper.Map(dto, chat);

            await _context.SaveChangesAsync();

            return Ok(new {message = "Chat name updated successfully", chatId=chat.Id, newName=chat.Name});
        }
    }
}
