using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UI.Customer.ViewModel;

namespace UI.Customer.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProfileController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AdminApi");
        }

        public async Task<IActionResult> Index()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Message = "Kullanıcı bilgisi bulunamadı.";
                return View(new List<PurchasedCourseViewModel>());
            }

            // 🔹 Kullanıcı bilgilerini getir
            var userResponse = await _httpClient.GetAsync($"api/Auth/GetUserByEmail?email={email}");
            if (!userResponse.IsSuccessStatusCode)
            {
                ViewBag.Message = "Kullanıcı bulunamadı.";
                return View(new List<PurchasedCourseViewModel>());
            }

            var userJson = await userResponse.Content.ReadAsStringAsync();
            dynamic user = JsonConvert.DeserializeObject(userJson);

            if (user == null)
            {
                ViewBag.Message = "Kullanıcı bilgisi okunamadı.";
                return View(new List<PurchasedCourseViewModel>());
            }

            int userId = user.id;
            string role = user.role;
            ViewBag.UserName = user.name;
            ViewBag.Role = role;

            List<PurchasedCourseViewModel> courses = new();

            if (role == "Admin")
            {
                // Admin → tüm kursları görür
                var courseResponse = await _httpClient.GetAsync("api/Course");
                if (courseResponse.IsSuccessStatusCode)
                {
                    var json = await courseResponse.Content.ReadAsStringAsync();
                    var allCourses = JsonConvert.DeserializeObject<List<PurchasedCourseViewModel>>(json) ?? new List<PurchasedCourseViewModel>();

                    // fallback resim
                    foreach (var c in allCourses)
                    {
                        if (string.IsNullOrEmpty(c.ImagePath))
                            c.ImagePath = $"https://picsum.photos/300/200?random={Guid.NewGuid()}";
                    }

                    courses = allCourses;
                }
            }
            else
            {
                // Öğrenci → sadece satın aldıkları
                var purchasedResponse = await _httpClient.GetAsync($"api/Purchase/User/{userId}");
                if (purchasedResponse.IsSuccessStatusCode)
                {
                    var purchasedJson = await purchasedResponse.Content.ReadAsStringAsync();
                    var purchasedCourses = JsonConvert.DeserializeObject<List<PurchasedCourseViewModel>>(purchasedJson) ?? new List<PurchasedCourseViewModel>();

                    foreach (var c in purchasedCourses)
                    {
                        if (string.IsNullOrEmpty(c.ImagePath))
                            c.ImagePath = $"https://picsum.photos/300/200?random={Guid.NewGuid()}";
                    }

                    courses = purchasedCourses;
                }
            }

            return View(courses);
        }
    }
}
