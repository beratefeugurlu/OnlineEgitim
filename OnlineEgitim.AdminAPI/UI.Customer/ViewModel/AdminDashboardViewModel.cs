using UI.Customer.Models;

namespace OnlineEgitim.UI.Customer.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalCourses { get; set; }
        public int PurchasedCount { get; set; }
        public int PendingApprovals { get; set; }

        public List<User> Users { get; set; } = new();
    }
}
