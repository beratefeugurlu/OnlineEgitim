namespace UI.Customer.ViewModel
{
    public class CourseCreateViewModel
    {
        public string Title { get; set; }
        public string Instructor { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; } // 📷 Fotoğraf dosyası
    }
}
