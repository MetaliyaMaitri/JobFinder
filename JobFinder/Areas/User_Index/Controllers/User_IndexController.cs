using JobFinder.Areas.Company.Models;
using JobFinder.Areas.Job.Models;
using JobFinder.Areas.State.Models;
using JobFinder.Areas.User_Index.Models;
using JobFinder.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using LOC_StateDropDownModel = JobFinder.Areas.State.Models.LOC_StateDropDownModel;

namespace JobFinder.Areas.User_Index.Controllers
{
    [Area("User_Index")]
    [Route("User_Index/{controller}/{action}")]
    public class User_IndexController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        #region Index


        public IActionResult Index(int StateID)
        {
           
            SetCityDropDownList();
            if (StateID == 0 || StateID == null)
            {
                return View();
            }
            else
            {
                User_Index_Base_DAL dal = new User_Index_Base_DAL();
                User_IndexModel model = dal.PR_LOC_State_SelectByPK((int)StateID);
                return View(model);
            }

        }
        #endregion

      

        #region #SetCityDropDownList
        public void SetCityDropDownList()
        {
            Company_Main_DAL lOC_DAL = new Company_Main_DAL();
            DataTable dt = lOC_DAL.Set_City_DropDownList();
            List<CityDropDownModel> cityyDropDownModelList = new List<CityDropDownModel>();
            foreach (DataRow data in dt.Rows)
            {
                CityDropDownModel cityDropDownModel = new CityDropDownModel();
                cityDropDownModel.CityID = Convert.ToInt32(data["CityID"]);
                cityDropDownModel.CityName = data["CityName"].ToString();
                cityyDropDownModelList.Add(cityDropDownModel);
            }
            ViewBag.SetCityDropDownList = cityyDropDownModelList;
        }
        #endregion

        #region UserCount
        public IActionResult UserCount()
        {
            User_Index_Base_DAL dashboard_Base_DAL = new User_Index_Base_DAL();
            ViewBag.CompanyCount = dashboard_Base_DAL.CompanyCount();
            ViewBag.JobCount = dashboard_Base_DAL.JobCount();
            ViewBag.CityCount = dashboard_Base_DAL.CityCount();
            ViewBag.UserCount = 10;
            return View();
        }
        #endregion

        #region UserCompanySelectAll

        public IActionResult CompanyList()
        {
            UserCompanyList_BASE_DAL dal = new UserCompanyList_BASE_DAL();

            return View(dal.CompanySelectAll());



        }

        #endregion

        #region AddEditCompany

        public IActionResult AddEditCompany(int CompanyID)
        {

            if (CompanyID == null)
            {
                return View();
            }
            else
            {
                UserCompanyList_BASE_DAL dal = new UserCompanyList_BASE_DAL();
                CompanyModel model = dal.PR_Company_SelectByPK(CompanyID);
                return View(model);
            }

        }
        #endregion

       
    }
}
