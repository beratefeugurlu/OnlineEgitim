using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineEgitim.UI.Customer.ViewModels;
using System.Text;
using UI.Customer.Models;   // ✅ Course buradan
using UI.Customer.ViewModel;

namespace UI.Customer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AdminApi");
        }

        // 📊 Dashboard
        public async Task<IActionResult> Dashboard()
        {
            int totalUsers = 0;
            int totalCourses = 0;
            int purchasedCount = 0;
            var users = new List<User>();

            // Kullanıcılar
            var userResponse = await _httpClient.GetAsync("api/Users");
            if (userResponse.IsSuccessStatusCode)
            {
                var json = await userResponse.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
                totalUsers = users.Count;
            }

            // Kurslar
            var courseResponse = await _httpClient.GetAsync("api/Course");
            if (courseResponse.IsSuccessStatusCode)
            {
                var json = await courseResponse.Content.ReadAsStringAsync();
                var courses = JsonConvert.DeserializeObject<List<Course>>(json);
                totalCourses = courses?.Count ?? 0;
            }

            // Satın alınan kurslar
            var purchaseResponse = await _httpClient.GetAsync("api/Purchase/All");
            if (purchaseResponse.IsSuccessStatusCode)
            {
                var json = await purchaseResponse.Content.ReadAsStringAsync();
                var purchases = JsonConvert.DeserializeObject<List<PurchasedCourseViewModel>>(json);
                purchasedCount = purchases?.Count ?? 0;
            }

            var vm = new AdminDashboardViewModel
            {
                TotalUsers = totalUsers,
                TotalCourses = totalCourses,
                PurchasedCount = purchasedCount,
                PendingApprovals = (int)(totalCourses * 0.2),
                Users = users
            };

            return View(vm);
        }

        // 👥 Kullanıcı listesi
        public async Task<IActionResult> Users()
        {
            var response = await _httpClient.GetAsync("api/Users");
            var users = new List<User>();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            }

            var model = users.Select(u => new UserViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role
            }).ToList();

            return View(model);
        }

        // ❌ Kullanıcı sil
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Users/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "✅ Kullanıcı başarıyla silindi.";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                TempData["Error"] = $"❌ Kullanıcı silinemedi. Hata: {error}";
            }
            return RedirectToAction("Users");
        }

        // 🎓 Kullanıcının satın aldığı kurslar
        public async Task<IActionResult> UserCourses(int id)
        {
            var response = await _httpClient.GetAsync($"api/Purchase/User/{id}");
            var purchasedCourses = new List<PurchasedCourseViewModel>();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                purchasedCourses = JsonConvert.DeserializeObject<List<PurchasedCourseViewModel>>(json) ?? new List<PurchasedCourseViewModel>();
            }

            return View(purchasedCourses);
        }

        // ➕ Kurs ekleme sayfası (GET)
        [HttpGet]
        public IActionResult AddCourse()
        {
            return View(new Course()); // ✅ Doğru model gönderiliyor
        }

        // ➕ Kurs ekleme işlemi (POST)
        [HttpPost]
        public async Task<IActionResult> AddCourse(Course course)
        {
            if (!ModelState.IsValid)
            {
                return View(course);
            }

            // Eğer resim boşsa random ekle
            if (string.IsNullOrEmpty(course.ImagePath))
            {
                course.ImagePath = $"https://picsum.photos/300/200?random={Guid.NewGuid()}";
            }

            var json = JsonConvert.SerializeObject(course);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Course", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "✅ Kurs başarıyla eklendi.";
                return RedirectToAction("Index", "Course"); // Courses sayfasına yönlendirme
            }

            TempData["Error"] = "❌ Kurs eklenirken hata oluştu.";
            return View(course);
        }
    }
}
