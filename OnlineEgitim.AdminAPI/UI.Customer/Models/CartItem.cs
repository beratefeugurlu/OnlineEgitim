namespace UI.Customer.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Instructor { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
