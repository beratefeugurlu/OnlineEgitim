using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using UI.Customer.Extensions;
using UI.Customer.Models;

namespace UI.Customer.Controllers
{
    [Authorize] // sadece giriş yapan kullanıcı ödeme yapabilsin
    public class PaymentController : Controller
    {
        private readonly HttpClient _httpClient;

        public PaymentController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AdminApi");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Kullanıcı giriş yapmamış.";
                return RedirectToAction("Login", "Account");
            }

            // Sepet session’dan alınır
            var cart = HttpContext.Session.GetObjectFromJson<List<Course>>("Cart") ?? new List<Course>();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Pay(string cardNumber, string expiryDate, string cvv)
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Kullanıcı giriş yapmamış.";
                return RedirectToAction("Login", "Account");
            }

            // Sepeti getir
            var cart = HttpContext.Session.GetObjectFromJson<List<Course>>("Cart") ?? new List<Course>();
            if (!cart.Any())
            {
                TempData["Error"] = "Sepetiniz boş!";
                return RedirectToAction("Index", "Cart");
            }

            var courseIds = cart.Select(c => c.Id).ToList();

            // AdminAPI'ye gönder
            var request = new
            {
                Email = email,
                CourseIds = courseIds
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Purchase/Buy", content);

            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.Remove("Cart");
                TempData["Success"] = "Ödeme başarılı! Kurslar profilinize eklendi.";
                return RedirectToAction("Index", "Profile");
            }
            else
            {
                TempData["Error"] = "Satın alma sırasında hata oluştu!";
                return RedirectToAction("Index", "Cart");
            }
        }
    }
}
