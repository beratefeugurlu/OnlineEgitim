using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Customer.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class MaterialController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public MaterialController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // Yükleme Sayfası
        public IActionResult Upload()
        {
            return View();
        }

        // Dosya Yükleme (POST)
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Error = "Lütfen bir dosya seçiniz.";
                return View();
            }

            // Uploads klasörüne kaydet
            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var filePath = Path.Combine(uploadsPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            ViewBag.Message = $"'{file.FileName}' başarıyla yüklendi (Admin onayına gönderildi).";
            return View();
        }
    }
}
