using MessageService.Data;
using Microsoft.AspNetCore.Mvc;
using MessageService.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MessageService.Mapping;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MessageService.Controllers
{
    [ApiController]
    [Route("api/chats")]
    public class ChatController : ControllerBase
    {
        private readonly MessageDbContext _context;
        private readonly IMapper _mapper;

        public ChatController(MessageDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatDTO chat)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (chat.User1Id != currentUserId && chat.User2Id != currentUserId)
            {
                return Forbid();
            }

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

        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetChatByUserId(int userId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (userId != currentUserId)
            {
                return Forbid();
            }

            var chats = await _context.Chats
                .Where(c => c.User1Id == userId || c.User2Id == userId)
                .ToListAsync();

            if (chats == null || chats.Count == 0)
            {
                return NotFound(new { message = "No chats found for this user" });
            }
            return Ok(chats);
        }

        [Authorize]
        [HttpGet("get/{chatId}")]
        public async Task<IActionResult> GetChatById(int chatId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat == null)
            {
                return NotFound(new { message = "Chat not found" });
            }

            if (chat.User1Id != currentUserId && chat.User2Id != currentUserId)
            {
                return Forbid();
            }

            return Ok(chat);
        }

        [Authorize]
        [HttpDelete("delete/{chatId}")]
        public async Task<IActionResult> DeleteChat(int chatId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat == null)
            {
                return NotFound(new { message = "Chat not found" });
            }

            if (chat.User1Id != currentUserId && chat.User2Id != currentUserId)
            {
                return Forbid();
            }

            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Chat deleted successfully" });
        }
        [Authorize]
        [HttpPut("{chatId}")]
        public async Task<IActionResult> UpdateChatName(int chatId, [FromBody] UpdateChatNameDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new { message = "Chat name cannot be empty" });
            }

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat == null)
            {
                return NotFound(new { message = "Chat not found" });
            }

            if (chat.User1Id != currentUserId && chat.User2Id != currentUserId)
            {
                return Forbid();
            }

            _mapper.Map(dto, chat);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Chat name updated successfully", chatId = chat.Id, newName = chat.Name });
        }
    }
}
