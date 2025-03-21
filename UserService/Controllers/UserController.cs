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
        public IActionResult Register([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.PasswordHash))
            {
                return BadRequest(new { message = "Wszystkie pola są wymagane!" });
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new
            {
                message = "Użytkownik został zarejestrowany!",
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
                return NotFound(new { message = "Użytkownik nie znaleziony!" });
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
                return NotFound(new { message = "Użytkownik nie znaleziony!" });
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok(new { message = "Użytkownik usunięty!" });
        }

       
        
    }
}
