using JobFinder.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;

namespace JobFinder.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Route("Dashboard/{controller}/{action}")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            Dashboard_Base_DAL dashboard_Base_DAL = new Dashboard_Base_DAL();
            ViewBag.CompanyCount = dashboard_Base_DAL.CompanyCount();
            ViewBag.JobCount = dashboard_Base_DAL.JobCount();
            ViewBag.CityCount = dashboard_Base_DAL.CityCount();
            ViewBag.UserCount = 10;
            return View();
        }
    }
}
