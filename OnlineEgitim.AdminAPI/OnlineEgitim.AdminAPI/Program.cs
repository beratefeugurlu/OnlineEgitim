using Microsoft.EntityFrameworkCore;
using OnlineEgitim.AdminAPI.Data;
using Microsoft.OpenApi.Models;
using OnlineEgitim.AdminAPI.Services;
using OnlineEgitim.AdminAPI.Settings;
using OnlineEgitim.AdminAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);


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


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<ITokenService, TokenService>();


builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdminAPI v1");
});


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
