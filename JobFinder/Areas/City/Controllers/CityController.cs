using ClosedXML.Excel;
using JobFinder.Areas.City.Models;
using JobFinder.Areas.Country.Models;
using JobFinder.Areas.State.Models;
using JobFinder.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

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

        #region Selectallfor excel
        public List<CityModel> GetCityModels()
        {
            List<CityModel> cityyModels = new List<CityModel>();
            string myconnStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(myconnStr);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_City_SelectAll";
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    CityModel cityModel = new CityModel
                    {
                        CityID = (int)reader["CityID"],
                        CityName = reader["CityName"].ToString(),
                        CityCode = reader["CityCode"].ToString(),
                        CountryName = reader["CountryName"].ToString(),
                        StateName = reader["StateName"].ToString(),
                        CreationDate = Convert.ToDateTime(reader["CreationDate"]),
                        Modified = Convert.ToDateTime(reader["Modified"]),



                    };
                    cityyModels.Add(cityModel);
                }
                return cityyModels;
            }
        }
        public IActionResult ExportCityToExcel()
        {

            List<CityModel> cityyModels = GetCityModels();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("CityModel");
                // Add headers
                worksheet.Cell(1, 1).Value = "CityID";
                worksheet.Cell(1, 2).Value = "CityName";
                worksheet.Cell(1, 3).Value = "CityCode";
                worksheet.Cell(1, 4).Value = "CountryName";
                worksheet.Cell(1, 5).Value = "StateName";
                worksheet.Cell(1, 6).Value = "CreationDate";
                worksheet.Cell(1, 7).Value = "Modified";

                // Add data
                int row = 2;
                foreach (var cityyModel in cityyModels)
                {
                    worksheet.Cell(row, 1).Value = cityyModel.CityID;
                    worksheet.Cell(row, 2).Value = cityyModel.CityName;
                    worksheet.Cell(row, 3).Value = cityyModel.CityCode;
                    worksheet.Cell(row, 4).Value = cityyModel.CountryName;
                    worksheet.Cell(row, 5).Value = cityyModel.StateName;
                    worksheet.Cell(row, 6).Value = cityyModel.CreationDate.ToString("yyyy-MM-dd");
                    worksheet.Cell(row, 7).Value = cityyModel.Modified.ToString("yyyy-MM-dd");


                    // Add other properties...
                    row++;
                }
                // Set content type and filename
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = "StudentData.xlsx";
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }
        #endregion


        [HttpPost]

        #region Multiple Delete
        public ActionResult Delete(int[] id)
        {

            foreach (var item in id)
            {
                try
                {
                    LOC_CityDelete(item);
                    Console.WriteLine("Deleted " + item);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return View("CountryList");
        }

        #endregion

    }
}
