using OnlineEgitim.AdminAPI.Models;

public class OrderItem
{
    public int Id { get; set; }

    // Order ilişkisi
    public int OrderId { get; set; }
    public Order Order { get; set; }

    // Course ilişkisi
    public int CourseId { get; set; }
    public Course Course { get; set; }

    public decimal Price { get; set; }

    // Sipariş edilen adet
    public int Quantity { get; set; }
}
