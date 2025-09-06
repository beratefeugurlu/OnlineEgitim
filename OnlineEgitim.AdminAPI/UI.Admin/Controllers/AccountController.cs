using Microsoft.AspNetCore.Mvc;

namespace UI.Admin.Controllers
{
    public class AccountController : Controller
    {
        // Admin giriş ekranı olmayacak, Customer UI yönlendirecek
        public IActionResult Login(string returnUrl = "/Admin/Dashboard")
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Redirect(returnUrl);
            }

            // Customer UI login sayfasına yönlendir
            return Redirect("https://localhost:7045/Account/Login?returnUrl=" + returnUrl);
        }

        public IActionResult Logout()
        {
            return Redirect("https://localhost:7045/Account/Logout");
        }
    }
}
