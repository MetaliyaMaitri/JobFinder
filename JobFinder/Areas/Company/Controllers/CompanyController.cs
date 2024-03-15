using ClosedXML.Excel;
using JobFinder.Areas.City.Models;
using JobFinder.Areas.Company.Models;
using JobFinder.Areas.Country.Models;
using JobFinder.Areas.State.Models;
using JobFinder.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
namespace JobFinder.Areas.Company.Controllers
{
    [Area("Company")]
    [Route("Company/{controller}/{action}")]
    public class CompanyController : Controller
    {
        #region Configuration
        private IConfiguration Configuration;
        public CompanyController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #endregion

        #region SelectAll

        public IActionResult CompanyList()
        {
            Company_Base_DAL dal = new Company_Base_DAL();

            return View(dal.CompanySelectAll());

          
           
        }

        #endregion

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
                //db.AddInParameter(dbCmd, "@EmploymentType", (DbType)SqlDbType.VarChar, CompanySearch.EmploymentType);
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

                    return View("CompanyList", modelList);
                }
            }
            catch (Exception ex)
            {

                return null;

            }
        }
        #endregion

        #region CompanyDelete
        public IActionResult DeleteCompany(int CompanyID)
        {

            Company_Base_DAL dal = new Company_Base_DAL();
            TempData["message"] = dal.DeleteCompany(CompanyID); ;
            return RedirectToAction("CompanyList");
        }
        #endregion

        //#region #CityAddEditView
        //public IActionResult Company()
        //{
        //    SetCityDropDownList();
        //    return View();
        //}
        //#endregion

        #region AddEditCompany

        public IActionResult AddEditCompany(int CompanyID)
        {
            SetCityDropDownList();
            if (CompanyID == null)
            {
                return View();
            }
            else
            {
                Company_Base_DAL dal = new Company_Base_DAL();
                CompanyModel model = dal.PR_Company_SelectByPK(CompanyID);
                return View(model);
            }

        }
        #endregion

        #region SaveForAddEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveForAddEdit(CompanyModel model)
        {
            SetCityDropDownList();
            bool ans = false;
            
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var path = Path.Combine(Environment.CurrentDirectory, "wwwroot", "Company", model.CompanyName + "." + model.ImageFile.ContentType.Split('/')[1]);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    model.ImageFile.CopyTo(stream);
                }
                model.ImageUrl = model.CompanyName + "." + model.ImageFile.ContentType.Split('/')[1];
            }
            else
            {
                return RedirectToAction("CompanyList");
            }
           
            Company_Base_DAL dal = new Company_Base_DAL();
            if (model.CompanyID != 0)
            {
                ans = dal.PR_Company_Update(model);
                TempData["message"] = "Record Updated Successfully";
            }
            else
            {
                ans = dal.PR_Company_Insert(model);
                TempData["message"] = "Record Inserted Successfully";
            }
            if (ans)
            {
                return RedirectToAction("CompanyList");
            }
            else
            {
                return RedirectToAction("CompanyList");
            }
        }
        #endregion

        #region CompanyDetails
        public IActionResult CompanyDetails(int CompanyID)
        {
            Company_Base_DAL bal = new Company_Base_DAL();
            CompanyModel model = bal.PR_Company_SelectByPK(CompanyID);
            return View(model);

        }
        #endregion

        #region #Cancel
        public IActionResult Cancel()
        {
            return RedirectToAction("CompanyList");
        }
        #endregion

        #region Selectallfor excel
        public List<CompanyModel> GetCompanyModels()
        {
            List<CompanyModel> companyModels = new List<CompanyModel>();
            string myconnStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(myconnStr);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Company_SelectAll";
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    CompanyModel companyModel = new CompanyModel
                    {
                        CompanyID = (int)reader["CompanyID"],
                        CompanyName = reader["CompanyName"].ToString(),
                        Size = (int)reader["Size"],
                        Description = reader["Description"].ToString(),
                        ContactEmail = reader["ContactEmail"].ToString(),
                        ContactPhone = reader["ContactPhone"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"]),



                    };
                    companyModels.Add(companyModel);
                }
                return companyModels;
            }
        }
        public IActionResult ExportCompanyToExcel()
        {

            List<CompanyModel> companyModels = GetCompanyModels();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("CompanyModel");
                // Add headers
                worksheet.Cell(1, 1).Value = "CompanyID";
                worksheet.Cell(1, 2).Value = "CompanyName";
                worksheet.Cell(1, 3).Value = "Size";
                worksheet.Cell(1, 4).Value = "Description";
                worksheet.Cell(1, 5).Value = "ContactEmail";
                worksheet.Cell(1, 6).Value = "ContactPhone";
                worksheet.Cell(1, 7).Value = "CreatedDate";
                worksheet.Cell(1, 8).Value = "ModifiedDate";

                // Add data
                int row = 2;
                foreach (var companyModel in companyModels)
                {
                    worksheet.Cell(row, 1).Value = companyModel.CompanyID;
                    worksheet.Cell(row, 2).Value = companyModel.CompanyName;
                    worksheet.Cell(row, 3).Value = companyModel.Size;
                    worksheet.Cell(row, 4).Value = companyModel.Description;
                    worksheet.Cell(row, 5).Value = companyModel.ContactEmail;
                    worksheet.Cell(row, 6).Value = companyModel.ContactPhone;
                    worksheet.Cell(row, 7).Value = companyModel.CreatedDate.ToString("yyyy-MM-dd");
                    worksheet.Cell(row, 8).Value = companyModel.ModifiedDate.ToString("yyyy-MM-dd");


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
                    DeleteCompany(item);
                    Console.WriteLine("Deleted " + item);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return View("CompanyList");
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
    }
}
