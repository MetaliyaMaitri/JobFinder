using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Areas.UserLogin.Controllers
{
    public class UserLoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
