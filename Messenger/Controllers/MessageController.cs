using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessageController : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("MessageController is working");
        }
    }
}
