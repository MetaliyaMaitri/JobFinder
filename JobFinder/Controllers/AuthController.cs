using JobFinder.DAL;
using JobFinder.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Controllers
{
    public class AuthController : Controller
    {

        #region LoginView
        public IActionResult Login()
        {
            return View();
        }
        #endregion
        [HttpPost]
        #region Login
        public IActionResult Login(Login login)
        {
            if (login.UserEmail != null && login.UserPassword != null)
            {
                AuthDAL auth = new AuthDAL();
                var User = auth.GetUserByEmail(login.UserEmail);
                if (User != null)
                {
                    if (User.UserPassword.Equals(login.UserPassword))
                    {
                        if (User.UserRole == "admin")
                        {

                            return RedirectToAction("Index", "Dashboard", new { area = "Dashboard" });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }


                else
                {
                    // user not found
                }
            }

            return View();
        }

        #endregion

        public IActionResult Register(int UserId, string UserRole)
        {
            AuthDAL dal = new AuthDAL();
            AuthModel model = dal.RegisterSelectByPk(UserId, UserRole);
            return View(model);
        }

        #region SaveForAddEdit
        public IActionResult RegisterSaveForAddEdit(AuthModel model)

        {



            bool ans = false;

            AuthDAL dal = new AuthDAL();
            if (model.UserId != 0)
            {
                ans = dal.UpdateUser(model);
                TempData["message"] = "Record Updated Successfully";

            }
            else
            {
                ans = dal.InsertUser(model);
                TempData["message"] = "Record Inserted Successfully";
            }
            if (ans)
            {
                return RedirectToAction("Index", "Home");

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }
        #endregion






    }
}
