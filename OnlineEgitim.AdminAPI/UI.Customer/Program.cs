using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OnlineEgitim.AdminAPI.Data; // ✅ AppDbContext erişimi için eklendi

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// ✅ HttpClient (AdminAPI çağrıları için) → appsettings.json’dan BaseUrl okunuyor, yoksa default port
builder.Services.AddHttpClient("AdminApi", client =>
{
    var apiUrl = builder.Configuration["ApiSettings:BaseUrl"];
    if (string.IsNullOrEmpty(apiUrl))
    {
        // Eğer appsettings.json’da tanımlı değilse default portu kullan
        apiUrl = "https://localhost:7279/";
    }

    client.BaseAddress = new Uri(apiUrl);
});

// ✅ DbContext ekleme
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ✅ Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// IConfiguration injection
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ Session aktif
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// Default route → Home/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
