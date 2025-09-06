namespace UI.Customer.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Instructor { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
        public string? ImagePath { get; set; }
    }
}
