using JobFinder.Areas.City.Models;
using JobFinder.Areas.Country.Models;
using JobFinder.Areas.State.Models;
using JobFinder.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace JobFinder.Areas.City.Controllers
{
    [Area("City")]
    [Route("City/{controller}/{action}")]
    public class CityController : Controller
    {
        #region Configuration
        private IConfiguration Configuration;
        public CityController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #endregion
        #region SelectAll

        public IActionResult CityList()
        {
            City_Base_DAL dal = new City_Base_DAL();
            return View(dal.CitySelectAll());

        }
        #endregion

        #region SelectAllSearch

        public IActionResult SearchCity(CitySearchModel CitySearch)
        {
            try
            {
                String connectionStr = this.Configuration.GetConnectionString("myConnectionString");
                Database db = new SqlDatabase(connectionStr);
                DbCommand dbCmd = db.GetStoredProcCommand("PR_LOC_City_Search");
                db.AddInParameter(dbCmd, "@CountryName", (DbType)SqlDbType.VarChar, CitySearch.CountryName);
                db.AddInParameter(dbCmd, "@StateName", (DbType)SqlDbType.VarChar, CitySearch.StateName);
                db.AddInParameter(dbCmd, "@CityName", (DbType)SqlDbType.VarChar, CitySearch.CityName);
                db.AddInParameter(dbCmd, "@CityCode", (DbType)SqlDbType.VarChar, CitySearch.CityCode);

                using (IDataReader dr = db.ExecuteReader(dbCmd))
                {
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    List<CityModel> modelList = new List<CityModel>();

                    foreach (DataRow row in dt.Rows)
                    {
                        CityModel city = new CityModel();
                        city.CityID = Convert.ToInt32(row["CityID"]);
                        city.CityName = row["CityName"].ToString();
                        city.CityCode = row["CityCode"].ToString();
                        city.CountryName = row["CountryName"].ToString();
                        city.StateName = row["StateName"].ToString();
                        city.CreationDate = Convert.ToDateTime(row["CreationDate"]);
                        city.Modified = Convert.ToDateTime(row["Modified"]);
                        // Set other properties here

                        modelList.Add(city);
                    }

                    return View("CityList", modelList);
                }
            }
            catch (Exception ex)
            {

                return null;

            }
        }
        #endregion

        #region #SetStateDropDownList
        public void SetStateDropDownList()
        {
            City_Main_DAL lOC_DAL = new City_Main_DAL();
            DataTable dt = lOC_DAL.Set_State_DropDownList();
            List<LOC_StateDropDownModel> stateDropDownModelList = new List<LOC_StateDropDownModel>();
            foreach (DataRow data in dt.Rows)
            {
                LOC_StateDropDownModel stateDropDownModel = new LOC_StateDropDownModel();
                stateDropDownModel.StateID = Convert.ToInt32(data["StateID"]);
                stateDropDownModel.StateName = data["StateName"].ToString();
                stateDropDownModelList.Add(stateDropDownModel);
            }
            ViewBag.SetStateDropDownList = stateDropDownModelList;
        }
        #endregion

        #region  SetCountryDropDownList
        public void SetCountryDropDownList()
        {
            City_Main_DAL lOC_DAL = new City_Main_DAL();
            DataTable dt = lOC_DAL.Set_Country_DropDownList();
            List<LOC_CountryDropDownModel> countryDropDownModelList = new List<LOC_CountryDropDownModel>();
            foreach (DataRow data in dt.Rows)
            {
                LOC_CountryDropDownModel countryDropDownModel = new LOC_CountryDropDownModel();
                countryDropDownModel.CountryID = Convert.ToInt32(data["CountryID"]);
                countryDropDownModel.CountryName = data["CountryName"].ToString();
                countryDropDownModelList.Add(countryDropDownModel);
            }
            ViewBag.SetCountryDropDownList = countryDropDownModelList;
        }
        #endregion

      

        #region LOC_CityAddEdit
       
        public IActionResult LOC_CityAddEdit(int CityID)
        {
            SetStateDropDownList();
            SetCountryDropDownList();
          

            if (CityID == null)
            {
                return View();
            }
            else
            {
                City_Base_DAL dal = new City_Base_DAL();
                CityModel model = dal.PR_LOC_City_SelectByPK(CityID);
                return View(model);
            }

        }
        #endregion


        #region SaveForAddEdit
        public IActionResult SaveForAddEdit(CityModel model)
        {

            //if (ModelState.IsValid)
            //{
                bool ans = false;
            SetStateDropDownList();
             SetCountryDropDownList();
            City_Base_DAL dal = new City_Base_DAL();
                if (model.CityID != 0)
                {
                    ans = dal.PR_LOC_City_Update(model);
                    TempData["message"] = "Record Updated Successfully";
                }
                else
                {
                    ans = dal.PR_LOC_City_Insert(model);
                    TempData["message"] = "Record Inserted Successfully";
                }
                if (ans)
                {
                    return RedirectToAction("CityList");
                }
                else
                {
                    return RedirectToAction("CityList");
                }

            //}
            //else 
            //{
            //   
            //    return View("LOC_CityAddEdit");
            //}
               
            
         
        }
        #endregion

        //#region CityDetails
        //public IActionResult CityDetails(int CityID)
        //{
        //    City_Base_DAL dal = new City_Base_DAL();
        //    CityModel model = dal.PR_LOC_City_SelectByPK(CityID);
        //    return View(model);

        //}
        //#endregion

        #region #CityDelete
        public IActionResult LOC_CityDelete(int CityID)
        {
            
            City_Base_DAL lOC_DAL = new City_Base_DAL();
            TempData["message"] = lOC_DAL.LOC_City_Delete(CityID); ;
            return RedirectToAction("CityList");
        }
        #endregion

        #region #Cancel
        public IActionResult Cancel()
        {
            return RedirectToAction("CityList");
        }
        #endregion
    }
}
