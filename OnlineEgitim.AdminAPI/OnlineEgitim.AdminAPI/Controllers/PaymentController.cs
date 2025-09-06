using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineEgitim.AdminAPI.Data;
using OnlineEgitim.AdminAPI.Models;

namespace OnlineEgitim.AdminAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Pay([FromBody] PaymentRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null) return BadRequest("Kullanıcı bulunamadı!");

            var cartItems = await _context.Carts
                .Where(c => c.UserId == user.Id)
                .ToListAsync();

            if (!cartItems.Any()) return BadRequest("Sepet boş!");

            foreach (var item in cartItems)
            {
                var purchased = new PurchasedCourse
                {
                    UserId = user.Id,
                    CourseId = item.CourseId,
                    PurchaseDate = DateTime.Now
                };

                _context.PurchasedCourses.Add(purchased);
            }

            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Ödeme başarılı, kurslar eklendi!" });
        }
    }

    public class PaymentRequest
    {
        public string Email { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;
    }
}
