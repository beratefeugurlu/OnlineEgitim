using Microsoft.EntityFrameworkCore;
using OnlineEgitim.AdminAPI.Data;
using Microsoft.OpenApi.Models;
using OnlineEgitim.AdminAPI.Services; // ✅ TokenService için
using OnlineEgitim.AdminAPI.Settings; // ✅ JwtSettings için
using OnlineEgitim.AdminAPI.Repositories; // ✅ Repository için

var builder = WebApplication.CreateBuilder(args);

// API only
builder.Services.AddControllers();

// Swagger setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OnlineEgitim.AdminAPI",
        Version = "v1"
    });
});

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Generic Repository ekle
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// ✅ Token Service ekle
builder.Services.AddScoped<ITokenService, TokenService>();

// ✅ JwtSettings ekle
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var app = builder.Build();

// Swagger aktif
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdminAPI v1");
});

// ✅ Middleware sırası
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// ✅ Controller endpointleri çalışsın
app.MapControllers();

app.Run();
