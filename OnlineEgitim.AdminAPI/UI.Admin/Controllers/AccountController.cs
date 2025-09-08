using Microsoft.AspNetCore.Mvc;

namespace UI.Admin.Controllers
{
    public class AccountController : Controller
    {
        // adminin ozel login sayfası yok , ortak bir login sisstemi bulunuyor
        public IActionResult Login(string returnUrl = "/Admin/Dashboard")
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Redirect(returnUrl);
            }
            
            return Redirect("https://localhost:7045/Account/Login?returnUrl=" + returnUrl);
        }

        public IActionResult Logout()
        {
            return Redirect("https://localhost:7045/Account/Logout");
        }
    }
}
