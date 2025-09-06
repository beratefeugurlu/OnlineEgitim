namespace OnlineEgitim.AdminAPI.Models
{
    public class PaymentRequest
    {
        public int UserId { get; set; }
        public List<int> CourseIds { get; set; } = new();
        public string CardNumber { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
    }
}
