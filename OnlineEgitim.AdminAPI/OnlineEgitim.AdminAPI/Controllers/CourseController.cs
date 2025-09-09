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

        // Tüm kursları getircek
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

        // ID’ye göre kurs getircek
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            if (string.IsNullOrEmpty(course.ImagePath))
                course.ImagePath = $"https://picsum.photos/300/200?random={Guid.NewGuid()}";

            return Ok(course);
        }

        // Yeni kurs ekleme
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Course model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest("Geçersiz kurs bilgisi!");

<<<<<<< HEAD
            
            model.CreatedDate = DateTime.Now;

            // resim kısmı boşsa rastgele resim gelecek ???
=======
            // Tarih otomatik
            model.CreatedDate = DateTime.Now;

            // Eğer resim boş ise random resim ata
>>>>>>> f50cba5124b193400b2f794d3e66b1416c53be7a
            if (string.IsNullOrEmpty(model.ImagePath))
                model.ImagePath = $"https://picsum.photos/300/200?random={Guid.NewGuid()}";

            
            model.IsApproved = true;

            _context.Courses.Add(model);
            await _context.SaveChangesAsync();

<<<<<<< HEAD
            
=======
            // ✅ Kurs başarıyla eklendi
>>>>>>> f50cba5124b193400b2f794d3e66b1416c53be7a
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        //  Kurs güncelleme
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

        //  Kurs silmee
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
