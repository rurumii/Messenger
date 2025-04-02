using Microsoft.AspNetCore.Mvc;
using UserService.Data;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _context;

        public UserController(UserDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegistrationDTO dto)
        {
           if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = dto.PasswordHash,
                UserTag = dto.UserTag
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new
            {
                message = "User has been registered!",
                userId = user.Id,
                username = user.Username,
                email = user.Email,
            });

        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _context.Users.Find(id);
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
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found!" });
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok(new { message = "User deleted!" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDto)
        {
            var user = _context.Users.FirstOrDefault(u =>
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
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.ToList();

            return Ok(users);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserDTO updatedUser)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            user.Username = updatedUser.Username;
            user.Email = updatedUser.Email;
            user.PasswordHash = updatedUser.PasswordHash;
            user.UserTag = updatedUser.UserTag;
            user.ProfileImageUrl = updatedUser.ProfileImageUrl;
            user.Bio = updatedUser.Bio;

            _context.SaveChanges();

            return Ok(new { message = "User updated succesfully" });
        }

        [HttpGet("by-tag/{tag}")]
        public IActionResult GetUserByTag(string tag)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserTag == tag);

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
