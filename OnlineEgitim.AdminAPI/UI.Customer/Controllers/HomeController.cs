using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Customer.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.UserName = User.Identity.Name;
                ViewBag.Role = User.IsInRole("Admin") ? "Admin" : "Student/Instructor";
            }
            else
            {
                ViewBag.UserName = "Ziyaretçi";
                ViewBag.Role = "Guest";
            }

            return View();
        }
    }
}
