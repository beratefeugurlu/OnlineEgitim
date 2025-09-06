using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Admin.Controllers
{
    [Authorize(Roles = "Admin")] // sadece Admin girebilir
    public class AdminController : Controller
    {
        // Dummy ders listesi (ileride API'den gelecek)
        private static List<dynamic> courses = new List<dynamic>
        {
            new { Id = 1, Title = "C# ile Backend Geliştirme", Instructor = "Efe Hoca", Price = 250, IsApproved = false },
            new { Id = 2, Title = "Angular ile Frontend Geliştirme", Instructor = "Berat Hoca", Price = 200, IsApproved = true },
            new { Id = 3, Title = "SQL Veritabanı Yönetimi", Instructor = "Ali Hoca", Price = 150, IsApproved = false }
        };

        public IActionResult Dashboard()
        {
            ViewBag.TotalCourses = courses.Count;
            ViewBag.ApprovedCourses = courses.Count(c => c.IsApproved);
            ViewBag.PendingCourses = courses.Count(c => !c.IsApproved);

            return View(courses.OrderBy(c => c.Id));
        }

        // Ders Onaylama
        public IActionResult Approve(int id)
        {
            var course = courses.FirstOrDefault(c => c.Id == id);
            if (course != null)
            {
                courses.Remove(course);
                courses.Add(new { course.Id, course.Title, course.Instructor, course.Price, IsApproved = true });
                TempData["Message"] = $"{course.Title} onaylandı ✅";
            }
            return RedirectToAction("Dashboard");
        }

        // Ders Silme
        public IActionResult Delete(int id)
        {
            var course = courses.FirstOrDefault(c => c.Id == id);
            if (course != null)
            {
                courses.Remove(course);
                TempData["Message"] = $"{course.Title} silindi ❌";
            }
            return RedirectToAction("Dashboard");
        }
    }
}
