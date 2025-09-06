using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineEgitim.AdminAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok("Burası herkese açık ");
        }

        [Authorize]
        [HttpGet("secret")]
        public IActionResult Secret()
        {
            return Ok("Tolken ile giriş yaptınız ");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            return Ok("Bu endpoint sadece Admin rolüne açik ");
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet("instructor-only")]
        public IActionResult InstructorOnly()
        {
            return Ok("Bu endpoint sadece Instructor rolüne açık ");
      
        }
    }
}

