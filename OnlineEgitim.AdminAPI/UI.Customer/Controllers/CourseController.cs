using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UI.Customer.Models;

namespace UI.Customer.Controllers
{
    public class CourseController : Controller
    {
        private readonly HttpClient _httpClient;

        public CourseController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AdminApi");
        }

        // 🔓 Herkes kursları görebilir
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/Course");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Kurslar yüklenemedi.";
                return View(new List<UI.Customer.Models.Course>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var courses = JsonConvert.DeserializeObject<List<UI.Customer.Models.Course>>(json)
                          ?? new List<UI.Customer.Models.Course>();

            foreach (var course in courses)
            {
                if (string.IsNullOrEmpty(course.ImagePath))
                    course.ImagePath = $"https://picsum.photos/300/200?random={Guid.NewGuid()}";
            }

            return View(courses);
        }


        // 🔒 Instructor veya Admin kurs ekleyebilir
        [Authorize(Roles = "Instructor,Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> Add(Course newCourse)
        {
            var json = JsonConvert.SerializeObject(newCourse);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Course", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = $"{newCourse.Title} kursu başarıyla eklendi ✅";
            }
            else
            {
                TempData["Error"] = "Kurs eklenirken hata oluştu.";
            }

            return RedirectToAction("Index");
        }
    }
}
