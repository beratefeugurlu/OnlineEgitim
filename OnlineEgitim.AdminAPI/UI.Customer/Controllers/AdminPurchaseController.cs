using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UI.Customer.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UI.Customer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPurchaseController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminPurchaseController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AdminApi");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/Purchase/All");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Kayıtlar alınamadı.";
                return View(new List<PurchasedCourseViewModel>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var purchases = JsonConvert.DeserializeObject<List<PurchasedCourseViewModel>>(json)
                            ?? new List<PurchasedCourseViewModel>();

            
            foreach (var p in purchases)
            {
                if (string.IsNullOrEmpty(p.ImagePath))
                    p.ImagePath = $"https://picsum.photos/300/200?random={Guid.NewGuid()}";
            }

            return View(purchases);
        }
    }
}
