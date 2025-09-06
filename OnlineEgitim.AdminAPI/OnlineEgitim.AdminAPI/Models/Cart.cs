namespace OnlineEgitim.AdminAPI.Models
{
    public class Cart
    {
        public int Id { get; set; }

        // Kullanıcı ilişkisi
        public int UserId { get; set; }
        public User User { get; set; } = new User();

        // Course ilişkisi
        public int CourseId { get; set; }
        public Course Course { get; set; } = new Course();

        // Sepet için adet
        public int Quantity { get; set; }
    }
}
