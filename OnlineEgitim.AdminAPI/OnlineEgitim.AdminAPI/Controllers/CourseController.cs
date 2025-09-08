using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineEgitim.AdminAPI.Data;
using OnlineEgitim.AdminAPI.Models;

namespace OnlineEgitim.AdminAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CourseController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Tüm kursları getir
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _context.Courses.ToListAsync();

            foreach (var course in courses)
            {
                if (string.IsNullOrEmpty(course.ImagePath))
                {
                    course.ImagePath = $"https://picsum.photos/300/200?random={Guid.NewGuid()}";
                }
            }

            return Ok(courses);
        }

        // ✅ ID’ye göre kurs getir
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            if (string.IsNullOrEmpty(course.ImagePath))
                course.ImagePath = $"https://picsum.photos/300/200?random={Guid.NewGuid()}";

            return Ok(course);
        }

        // ✅ Yeni kurs ekle
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Course model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest("Geçersiz kurs bilgisi!");

            // Tarih otomatik
            model.CreatedDate = DateTime.Now;

            // Eğer resim boş ise random resim ata
            if (string.IsNullOrEmpty(model.ImagePath))
                model.ImagePath = $"https://picsum.photos/300/200?random={Guid.NewGuid()}";

            // Admin eklediği için direkt onaylı gelsin
            model.IsApproved = true;

            _context.Courses.Add(model);
            await _context.SaveChangesAsync();

            // ✅ Kurs başarıyla eklendi
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        // ✅ Kurs güncelle
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Course updatedCourse)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            course.Title = updatedCourse.Title;
            course.Description = updatedCourse.Description;
            course.Price = updatedCourse.Price;
            course.Instructor = updatedCourse.Instructor;
            course.IsApproved = updatedCourse.IsApproved;

            if (!string.IsNullOrEmpty(updatedCourse.ImagePath))
            {
                course.ImagePath = updatedCourse.ImagePath;
            }

            await _context.SaveChangesAsync();
            return Ok(course);
        }

        // ✅ Kurs sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Kurs silindi!" });
        }
    }
}
