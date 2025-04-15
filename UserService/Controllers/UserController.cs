using Microsoft.AspNetCore.Mvc;
using UserService.Data;
using UserService.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using UserService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace UserService.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase    
    {
        private readonly UserDbContext _context;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;

        public UserController(UserDbContext context, IMapper mapper, JwtService jwtService)
        {
            _context = context;
            _mapper = mapper;
            _jwtService = jwtService;
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
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, dto.Password);

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

            var userDto = _mapper.Map<PublicUserDto>(user);
            return Ok(userDto);
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
            u.Email == loginDto.Email);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var token = _jwtService.GenerateToken(user.Id, user.Username, user.Role);

            return Ok(new
            {
                message = "Login successfull",
                token = token,
                userId = user.Id,
                username = user.Username,
            });
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            return Ok(users);
        }

        [Authorize]
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
        [HttpPost("friends")]
        public async Task<IActionResult> AddFriend([FromBody] AddFriendDto dto)
        {
            if (dto.UserId == dto.FriendUserId)
            {
                return BadRequest(new { message = "You can't add yourself as a friend" });
            }
            var user = await _context.Users.FindAsync(dto.UserId);
            var friend = await _context.Friends.FindAsync(dto.FriendUserId);

            if (user == null || friend == null)
            {
                return NotFound(new { message = "User not found" });
            }
            
            var existingFriends = await _context.Friends.FirstOrDefaultAsync(f => 
            f.UserId == dto.UserId && f.FriendUserId == dto.FriendUserId);

            if (existingFriends != null)
            {
                return Conflict(new { message = "Already friends" });
            }
            var friendship = new Friend
            {
                UserId = dto.UserId,
                FriendUserId = dto.FriendUserId,
            };

            await _context.Friends.AddAsync(friendship);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Friend added successfully" });
        }

        [HttpDelete("{userId}/{friendUserId")]
        public async Task<IActionResult> DeleteFriend(int userId, int friendUserId)
        {
            var friendship = await _context.Friends.FirstOrDefaultAsync(f =>
            f.UserId == userId && f.FriendUserId == friendUserId);

            if (friendship == null)
            {
                return NotFound(new { message = "Frienship not found" });
            }

            _context.Friends.Remove(friendship);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Friend removed" });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFriendshipByUserId (int userId)
        {
            var friends = await _context.Friends
                .Where(f => f.UserId == userId)
                .Select(f => new
                {
                    f.FriendUserId,
                    f.FriendUser.Username,
                    f.FriendUser.Email
                })
                .ToListAsync();
            if (friends.Count == 0)
            {
                return NotFound(new { message = "No friends found" };
            }

            return Ok(friends);
        }
    }
}
