using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineEgitim.AdminAPI.Data;
using OnlineEgitim.AdminAPI.Models;

namespace OnlineEgitim.AdminAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PurchaseController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Kurs satın alma işlemi
        [HttpPost("Buy")]
        public async Task<IActionResult> Buy([FromBody] PurchaseRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı!");

            foreach (var courseId in request.CourseIds)
            {
                _context.PurchasedCourses.Add(new PurchasedCourse
                {
                    UserId = user.Id,
                    CourseId = courseId,
                    PurchaseDate = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Satın alma başarılı" });
        }

        // ✅ Tüm satın alınan kurslar (Admin için)
        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var purchases = await _context.PurchasedCourses
                .Include(pc => pc.User)
                .Include(pc => pc.Course)
                .Select(pc => new
                {
                    UserName = pc.User.Name,
                    UserEmail = pc.User.Email,
                    CourseTitle = pc.Course.Title,
                    CoursePrice = pc.Course.Price,
                    PurchaseDate = pc.PurchaseDate,
                    ImagePath = string.IsNullOrEmpty(pc.Course.ImagePath)
                        ? $"https://picsum.photos/300/200?random={Guid.NewGuid()}"
                        : pc.Course.ImagePath
                })
                .ToListAsync();

            return Ok(purchases);
        }

        // ✅ Belirli kullanıcının satın aldığı kurslar
        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var purchases = await _context.PurchasedCourses
                .Include(pc => pc.User)
                .Include(pc => pc.Course)
                .Where(pc => pc.UserId == userId)
                .Select(pc => new
                {
                    UserName = pc.User.Name,
                    UserEmail = pc.User.Email,
                    CourseTitle = pc.Course.Title,
                    CoursePrice = pc.Course.Price,
                    PurchaseDate = pc.PurchaseDate,
                    ImagePath = string.IsNullOrEmpty(pc.Course.ImagePath)
                        ? $"https://picsum.photos/300/200?random={Guid.NewGuid()}"
                        : pc.Course.ImagePath
                })
                .ToListAsync();

            return Ok(purchases);
        }
    }

    public class PurchaseRequest
    {
        public string Email { get; set; } = string.Empty;   
        public List<int> CourseIds { get; set; } = new List<int>();
    }
}
