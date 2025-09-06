using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Admin.Controllers
{
    [Authorize(Roles = "Admin")] // sadece admin erişir
    public class MaterialController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private static List<dynamic> materials = new List<dynamic>();

        public MaterialController(IWebHostEnvironment env)
        {
            _env = env;

            // wwwroot/uploads klasörünü oku (Instructor yükledikleri)
            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
            if (Directory.Exists(uploadsPath))
            {
                var files = Directory.GetFiles(uploadsPath);
                materials = files.Select((f, index) => new
                {
                    Id = index + 1,
                    FileName = Path.GetFileName(f),
                    Path = "/uploads/" + Path.GetFileName(f),
                    IsApproved = false
                }).ToList<dynamic>();
            }
        }

        public IActionResult Index()
        {
            return View(materials);
        }

        public IActionResult Approve(int id)
        {
            var file = materials.FirstOrDefault(m => m.Id == id);
            if (file != null)
            {
                materials.Remove(file);
                materials.Add(new { file.Id, file.FileName, file.Path, IsApproved = true });
                TempData["Message"] = $"{file.FileName} onaylandı!";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var file = materials.FirstOrDefault(m => m.Id == id);
            if (file != null)
            {
                // Fiziksel dosyayı sil
                var fullPath = Path.Combine(_env.WebRootPath, "uploads", file.FileName);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                materials.Remove(file);
                TempData["Message"] = $"{file.FileName} silindi!";
            }
            return RedirectToAction("Index");
        }
    }
}
