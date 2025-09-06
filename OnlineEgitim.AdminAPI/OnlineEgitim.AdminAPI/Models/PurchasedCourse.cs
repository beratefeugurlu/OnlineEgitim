namespace OnlineEgitim.AdminAPI.Models
{
    public class PurchasedCourse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime PurchaseDate { get; set; }

        public User User { get; set; }
        public Course Course { get; set; }
    }
}
