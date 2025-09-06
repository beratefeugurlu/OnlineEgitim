using OnlineEgitim.AdminAPI.Models;
using System.ComponentModel.DataAnnotations;

public class Course
{
    public int Id { get; set; }

    // Kurs başlığı
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    // Eğitmen bilgisi
    [Required, MaxLength(100)]
    public string Instructor { get; set; } = string.Empty;

    // Fiyat
    [Range(0, 99999)]
    public decimal Price { get; set; }

    // Açıklama
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    // ✅ Admin onayı
    public bool IsApproved { get; set; } = false;

    // ✅ Kurs görseli yolu
    [MaxLength(300)]
    public string? ImagePath { get; set; }

    // OrderItem ilişkisi
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    // ✅ Kursu satın alan kullanıcılar
    public ICollection<PurchasedCourse> PurchasedCourses { get; set; } = new List<PurchasedCourse>();
}
