using ClosedXML.Excel;
using JobFinder.Areas.Country.Models;

using JobFinder.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace JobFinder.Areas.Country.Controllers
{
    [Area("Country")]
    [Route("Country/{controller}/{action}")]
    public class CountryController : Controller
    {

        #region Configuration
        private IConfiguration Configuration;
        public CountryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #endregion


        #region Selectallfor excel
        public List<CountryModel> GetCountryModels()
        {
            List<CountryModel> countryModels = new List<CountryModel>();
            string myconnStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(myconnStr);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_Country_SelectAll";
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    CountryModel countryModel = new CountryModel
                    {
                        CountryID = (int)reader["CountryID"],
                        CountryName = reader["CountryName"].ToString(),
                        CountryCode = reader["CountryCode"].ToString(),
                        Created = Convert.ToDateTime(reader["Created"]),
                        Modified = Convert.ToDateTime(reader["Modified"]),



                    };
                    countryModels.Add(countryModel);
                }
                return countryModels;
            }
        }
        public IActionResult ExportCountryToExcel()
        {

            List<CountryModel> countryModels = GetCountryModels();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("CountryModel");
                // Add headers
                worksheet.Cell(1, 1).Value = "CountryID";
                worksheet.Cell(1, 2).Value = "CountryName";
                worksheet.Cell(1, 3).Value = "CountryCode";
                worksheet.Cell(1, 4).Value = "Created";
                worksheet.Cell(1, 5).Value = "Modified";

                // Add data
                int row = 2;
                foreach (var countryModel in countryModels)
                {
                    worksheet.Cell(row, 1).Value = countryModel.CountryID;
                    worksheet.Cell(row, 2).Value = countryModel.CountryName;
                    worksheet.Cell(row, 3).Value = countryModel.CountryCode;
                    worksheet.Cell(row, 4).Value = countryModel.Created.ToString("yyyy-MM-dd");
                    worksheet.Cell(row, 5).Value = countryModel.Modified.ToString("yyyy-MM-dd");


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
                    Delete(item);
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



        #region SelectAll

        public IActionResult CountryList()
        {
            Country_Base_DAL dal = new Country_Base_DAL();
            return View(dal.CountrySelectAll());

        }
        #endregion

        #region SelectAllSearch

        public IActionResult SearchCountry(CountrySearchModel countrySearch)
        {
            try
            {
                String connectionStr = this.Configuration.GetConnectionString("myConnectionString");
                Database db = new SqlDatabase(connectionStr);
                DbCommand dbCmd = db.GetStoredProcCommand("PR_LOC_Country_Filter");
                db.AddInParameter(dbCmd, "@CountryName", (DbType)SqlDbType.VarChar, countrySearch.CountryName);
                db.AddInParameter(dbCmd, "@CountryCode", (DbType)SqlDbType.VarChar, countrySearch.CountryCode);



                using (IDataReader dr = db.ExecuteReader(dbCmd))
                {
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    List<CountryModel> modelList = new List<CountryModel>();

                    foreach (DataRow row in dt.Rows)
                    {
                        CountryModel country = new CountryModel();
                        country.CountryID = Convert.ToInt32(row["CountryID"]);
                        country.CountryName = row["CountryName"].ToString();
                        country.CountryCode = row["CountryCode"].ToString();
                        country.Created = Convert.ToDateTime(row["Created"]);
                        country.Modified = Convert.ToDateTime(row["Modified"]);
                        // Set other properties here

                        modelList.Add(country);
                    }

                    return View("CountryList", modelList);
                }
            }
            catch (Exception ex)
            {

                return null;

            }
        }
        #endregion

        #region #CountryDelete
        public IActionResult LOC_CountryDelete(int CountryID)
        {

            Country_Base_DAL lOC_DAL = new Country_Base_DAL();
            TempData["message"] = lOC_DAL.LOC_Country_Delete(CountryID); ;
            return RedirectToAction("CountryList");
        }
        #endregion

        #region #Delete
        public IActionResult Delete(int CountryID)
        {

            Country_Base_DAL lOC_DAL = new Country_Base_DAL();
            TempData["message"] = lOC_DAL.LOC_Country_Delete(CountryID); ;
            return View();
        }
        #endregion

        #region LOC_CountryAddEdit
        public IActionResult LOC_CountryAddEdit(int CountryID)
        {
            if (CountryID == null)
            {
                return View();
            }
            else
            {
                Country_Base_DAL dal = new Country_Base_DAL();
                CountryModel model = dal.PR_LOC_Country_SelectByPK(CountryID);
                return View(model);
            }

        }
        #endregion

        #region SaveForAddEdit
        public IActionResult SaveForAddEdit(CountryModel model)
        {

            //if(ModelState.IsValid)
            //{
            bool ans = false;
            Console.WriteLine(model.CountryID);
            Country_Base_DAL dal = new Country_Base_DAL();
            if (model.CountryID != 0)
            {
                ans = dal.PR_LOC_Country_Update(model);
                TempData["message"] = "Record Updated Successfully";
            }
            else
            {
                ans = dal.PR_LOC_Country_Insert(model);
                TempData["message"] = "Record Inserted Successfully";
            }
            if (ans)
            {
                return RedirectToAction("CountryList");
            }
            else
            {
                return RedirectToAction("CountryList");
            }
            //}
            //else
            //{
            //    return View("LOC_CountryAddEdit");
            //}

        }
        #endregion

        #region CountryDetails
        public IActionResult CountryDetails(int CountryID)
        {
            Country_Base_DAL bal = new Country_Base_DAL();
            CountryModel model = bal.PR_LOC_Country_SelectByPK(CountryID);
            return View(model);

        }
        #endregion

        #region #Cancel
        public IActionResult Cancel()
        {
            return RedirectToAction("CountryList");
        }
        #endregion
    }
}
