using JobFinder.Areas.City.Models;
using JobFinder.Areas.Company.Models;
using JobFinder.Areas.User_Index.Models;
using JobFinder.DAL;
using JobFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Diagnostics;
using System.Data.Common;

namespace JobFinder.Controllers
{

    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        #region Configuration
        private IConfiguration Configuration;
        public HomeController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #endregion
        public IActionResult Index(int StateID)
        {
            SetCityDropDownList();

            ViewBag.CompanyList = CompanyList();
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

        #region SelectAllSearch

        public IActionResult SearchCompany(CompanySearchModel CompanySearch)
        {
            try
            {
                String connectionStr = this.Configuration.GetConnectionString("myConnectionString");
                Database db = new SqlDatabase(connectionStr);
                DbCommand dbCmd = db.GetStoredProcCommand("PR_Company_Search");
                db.AddInParameter(dbCmd, "@CompanyName", (DbType)SqlDbType.VarChar, CompanySearch.CompanyName);
                db.AddInParameter(dbCmd, "@CityName", (DbType)SqlDbType.VarChar, CompanySearch.CityName);

                using (IDataReader dr = db.ExecuteReader(dbCmd))
                {
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    List<CompanyModel> modelList = new List<CompanyModel>();

                    foreach (DataRow row in dt.Rows)
                    {
                        CompanyModel company = new CompanyModel();
                        company.CompanyID = Convert.ToInt32(row["CompanyID"]);
                        company.CompanyName = row["CompanyName"].ToString();
                        company.Size = Convert.ToInt32(row["Size"]);
                        company.CityID = Convert.ToInt32(row["CityID"]);
                        company.CityName = row["CityName"].ToString();
                        company.Description = row["Description"].ToString();
                        company.ContactEmail = row["ContactEmail"].ToString();
                        company.ContactPhone = row["ContactPhone"].ToString();
                        company.ImageUrl = row["ImageUrl"].ToString();
                        company.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                        company.ModifiedDate = Convert.ToDateTime(row["ModifiedDate"]);
                        // Set other properties here

                        modelList.Add(company);
                    }

                    // Set ViewBag.CompanyList with the populated list
                    ViewBag.CompanyList = modelList;
                    SetCityDropDownList();
                    // Return the list of companies to the Index view
                    return View("Index", modelList);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                // You might want to handle the exception in a more meaningful way, such as displaying an error message to the user
                return RedirectToAction("Index", "Home"); // Redirect to a meaningful action in case of an error
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
        public List<CompanyModel> CompanyList()
        {
            UserCompanyList_BASE_DAL dal = new UserCompanyList_BASE_DAL();

            return dal.CompanySelectAll();



        }
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
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}