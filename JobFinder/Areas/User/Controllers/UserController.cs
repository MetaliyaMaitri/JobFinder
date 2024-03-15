using JobFinder.DAL;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Areas.User.Controllers
{
    [Area("User")]
    [Route("User/{controller}/{action}")]
    public class UserController : Controller
    {
        #region SelectAll

        public IActionResult UserList()
        {
            User_Base_DAL dal = new User_Base_DAL();
            return View(dal.UserSelectAll());

        }
        #endregion
    }
}
