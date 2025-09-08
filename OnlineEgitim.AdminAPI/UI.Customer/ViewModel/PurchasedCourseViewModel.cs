using System;

namespace UI.Customer.ViewModel
{
    public class PurchasedCourseViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;

        public string CourseTitle { get; set; } = string.Empty;
        public decimal CoursePrice { get; set; }
        public DateTime PurchaseDate { get; set; }

      
        public string? ImagePath { get; set; }
    }
}
