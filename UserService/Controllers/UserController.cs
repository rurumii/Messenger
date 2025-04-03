using Microsoft.AspNetCore.Mvc;
using UserService.Data;
using UserService.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace UserService.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase    
    {
        private readonly UserDbContext _context;
        private readonly IMapper _mapper;

        public UserController(UserDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDTO dto)
        {
           if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           if (await _context.Users.AnyAsync(u => u.Email == dto.Email ))
            {
                return Conflict(new { message = "User with this email already exists" });
            }

            var user = _mapper.Map<User>(dto);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "User has been registered!",
                userId = user.Id,
                username = user.Username,
                email = user.Email,
            });

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found!" });
            }

            return Ok(new
            {
                id = user.Id,
                username = user.Username,
                email = user.Email,
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found!" });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "User deleted!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Email == loginDto.Email &&
            u.PasswordHash == loginDto.PasswordHash);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            return Ok(new
            {
                message = "Logged in successfully!",
                userId = user.Id,
                username = user.Username
            });

        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDTO updatedUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            
            _mapper.Map(updatedUser, user);

            await _context.SaveChangesAsync();

            return Ok(new { message = "User updated succesfully" });
        }

        [HttpGet("by-tag/{tag}")]
        public async Task<IActionResult> GetUserByTag(string tag)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserTag == tag);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new
            {
                id = user.Id,
                username = user.Username,
                userTag = user.UserTag,
                email = user.Email,
                bio = user.Bio,
                profileImageUrl = user.ProfileImageUrl
            });
        }
    }
}
