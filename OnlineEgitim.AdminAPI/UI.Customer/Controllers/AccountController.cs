using UI.Customer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace UI.Customer.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7279/api/");
        }

    
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var loginRequest = new { Email = email, Password = password };
            var content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "❌ Hatalı e-posta veya şifre!";
                return RedirectToAction("Login");
            }

            var tokenResponse = await response.Content.ReadAsStringAsync();
            string token;
            try
            {
                var json = JsonDocument.Parse(tokenResponse);
                token = json.RootElement.GetProperty("token").GetString() ?? "";
            }
            catch
            {
                token = tokenResponse; // fallback
            }

         
            string role = email.ToLower().Contains("admin") ? "Admin" : "Student";

         
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, role),
                new Claim("Token", token)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

           
            if (!ActiveUsers.Users.Contains(email))
                ActiveUsers.Users.Add(email);

            TempData["Success"] = $"👋 Hoş geldin {email}!";

            // Rol bazlı yönlendirme
            if (role == "Admin")
                return RedirectToAction("Dashboard", "Admin");

            return RedirectToAction("Index", "Home");
        }


        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

   
        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password)
        {
            var registerRequest = new { Name = name, Email = email, Password = password };
            var content = new StringContent(JsonSerializer.Serialize(registerRequest), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "✅ Kayıt başarılı! Şimdi giriş yapabilirsiniz.";
                return RedirectToAction("Login");
            }

            TempData["Error"] = "❌ Kayıt başarısız! Lütfen tekrar deneyin.";
            return RedirectToAction("Register");
        }

        // Logout işlemi    
        public async Task<IActionResult> Logout()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                // Kullanıcı aktif listeden çıkar
                var email = User.Identity?.Name;
                if (!string.IsNullOrEmpty(email) && ActiveUsers.Users.Contains(email))
                {
                    ActiveUsers.Users.Remove(email);
                }
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Success"] = "👋 Başarıyla çıkış yaptınız.";
            return RedirectToAction("Login");
        }

        
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

