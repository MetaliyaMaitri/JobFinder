using JobFinder.DAL;
using JobFinder.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Controllers
{
    public class AuthController : Controller
    {

        private readonly AuthDAL _auth;



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

        public IActionResult Register()
        {

            return View();
        }


        #region SaveForAddEdit
        public IActionResult SE_Register(Register model)
        {
            if (ModelState.IsValid)
            {
                AuthDAL DAL = new AuthDAL();
                DAL.InsertUser(model);
                TempData["Login"] = "Accountry Created Login To Continue....";
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
