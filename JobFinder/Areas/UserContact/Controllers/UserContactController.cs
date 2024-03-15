using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Areas.UserContact.Controllers
{
    public class UserContactController : Controller
    {
        [Area("UserContact")]
        [Route("UserContact/{controller}/{action}")]
        public IActionResult UserContactlist()
        {
            return View();
        }
    }
}
