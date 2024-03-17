using ClosedXML.Excel;
using JobFinder.Areas.Company.Models;
using JobFinder.Areas.Job.Models;
using JobFinder.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace JobFinder.Areas.Job.Controllers
{
    [Area("Job")]
    [Route("Job/{controller}/{action}")]
    public class JobController : Controller
    {
        #region Configuration
        private IConfiguration Configuration;
        public JobController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #endregion
        #region SelectAll

        public IActionResult JobList()
        {
            Job_Base_DAL dal = new Job_Base_DAL();
            return View(dal.JobSelectAll());

        }
        #endregion

        #region SelectAllSearch

        public IActionResult SearchJob(JobSearchModel JobSearch)
        {
            try
            {
                String connectionStr = this.Configuration.GetConnectionString("myConnectionString");
                Database db = new SqlDatabase(connectionStr);
                DbCommand dbCmd = db.GetStoredProcCommand("PR_Job_Search");
                db.AddInParameter(dbCmd, "@JobType", (DbType)SqlDbType.VarChar, JobSearch.JobType);
                db.AddInParameter(dbCmd, "@CompanyName", (DbType)SqlDbType.VarChar, JobSearch.CompanyName);
                db.AddInParameter(dbCmd, "@Location", (DbType)SqlDbType.VarChar, JobSearch.Location);
                db.AddInParameter(dbCmd, "@Salary", (DbType)SqlDbType.VarChar, JobSearch.Salary);

                using (IDataReader dr = db.ExecuteReader(dbCmd))
                {
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    List<JobModel> modelList = new List<JobModel>();

                    foreach (DataRow row in dt.Rows)
                    {
                        JobModel job = new JobModel();
                        job.JobID = Convert.ToInt32(row["JobID"]);
                        job.JobType = row["JobType"].ToString();
                        job.CompanyName = row["CompanyName"].ToString();
                        job.Location = row["Location"].ToString();
                        job.Requirements = row["Requirements"].ToString();
                        job.Salary = row["Salary"].ToString();
                        job.EmploymentType = row["EmploymentType"].ToString();
                        job.ExperienceLeval = row["ExperienceLeval"].ToString();
                        job.EducationLeval = row["EducationLeval"].ToString();

                        job.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                        job.ModifiedDate = Convert.ToDateTime(row["ModifiedDate"]);
                        // Set other properties here

                        modelList.Add(job);
                    }

                    return View("JobList", modelList);
                }
            }
            catch (Exception ex)
            {

                return null;

            }
        }
        #endregion

        #region #SetStateDropDownList
        public void SetCompanyDropDownList()
        {
            Job_Main_DAL lOC_DAL = new Job_Main_DAL();
            DataTable dt = lOC_DAL.Set_Company_DropDownList();
            List<CompanyDropDownModel> CompanyDropDownModelList = new List<CompanyDropDownModel>();
            foreach (DataRow data in dt.Rows)
            {
                CompanyDropDownModel companyDropDownModel = new CompanyDropDownModel();
                companyDropDownModel.CompanyID = Convert.ToInt32(data["CompanyID"]);
                companyDropDownModel.CompanyName = data["CompanyName"].ToString();
                CompanyDropDownModelList.Add(companyDropDownModel);
            }
            ViewBag.SetCompanyDropDownList = CompanyDropDownModelList;
        }
        #endregion


        #region JobAddEdit
        //[ValidateAntiForgeryToken]
        public IActionResult JobAddEdit(int JobID)
        {
            SetCompanyDropDownList();
            if (JobID == 0)
            {
                return View();
            }
            else
            {
                Job_Base_DAL dal = new Job_Base_DAL();
                JobModel model = dal.PR_Job_SelectByPK(JobID);
                return View(model);
            }

        }
        #endregion

        #region SaveForAddEdit
        public IActionResult SaveForAddEdit(JobModel model)

        {
            SetCompanyDropDownList();

            //if (ModelState.IsValid)
            //{

            bool ans = false;
            Console.WriteLine(model.JobID);
            Job_Base_DAL dal = new Job_Base_DAL();
            if (model.JobID != 0)
            {
                ans = dal.PR_Job_Update(model);
                TempData["message"] = "Record Updated Successfully";

            }
            else
            {
                ans = dal.PR_Job_Insert(model);
                TempData["message"] = "Record Inserted Successfully";
            }
            if (ans)
            {
                return RedirectToAction("JobList");

            }
            else
            {
                return RedirectToAction("JobList");
            }
            //}
            //else
            //{
            //    return View("JobAddEdit");
            //}

        }
        #endregion

        #region JobDetails
        public IActionResult JobDetails(int JobID)
        {
            Job_Base_DAL dal = new Job_Base_DAL();
            JobModel model = dal.PR_Job_SelectByPK(JobID);
            return View(model);

        }
        #endregion

        #region JobDelete
        public IActionResult JobDelete(int JobID)
        {

            Job_Base_DAL lOC_DAL = new Job_Base_DAL();
            TempData["message"] = lOC_DAL.JobDelete(JobID); ;
            return RedirectToAction("JobList");
        }
        #endregion


        #region Multiple Delete
        public ActionResult Delete(int[] id)
        {

            foreach (var item in id)
            {
                try
                {
                    JobDelete(item);
                    Console.WriteLine("Deleted " + item);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return View("JobList");
        }

        #endregion

        #region Selectallfor excel
        public List<JobModel> GetJobModels()
        {
            List<JobModel> jobModels = new List<JobModel>();
            string myconnStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(myconnStr);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Job_SelectAll";
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    JobModel jobModel = new JobModel
                    {
                        JobID = (int)reader["JobID"],
                        JobType = reader["JobType"].ToString(),
                        CompanyName = reader["CompanyName"].ToString(),
                        Location = reader["Location"].ToString(),
                        Requirements = reader["Requirements"].ToString(),
                        Salary = reader["Salary"].ToString(),
                        EmploymentType = reader["EmploymentType"].ToString(),
                        ExperienceLeval = reader["ExperienceLeval"].ToString(),
                        EducationLeval = reader["EducationLeval"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"]),



                    };
                    jobModels.Add(jobModel);
                }
                return jobModels;
            }
        }
        public IActionResult ExportJobToExcel()
        {

            List<JobModel> jobModels = GetJobModels();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("JobModel");
                // Add headers
                worksheet.Cell(1, 1).Value = "JobID";
                worksheet.Cell(1, 2).Value = "JobType";
                worksheet.Cell(1, 3).Value = "CompanyName";
                worksheet.Cell(1, 4).Value = "Location";
                worksheet.Cell(1, 5).Value = "Requirements";
                worksheet.Cell(1, 6).Value = "Salary";
                worksheet.Cell(1, 7).Value = "EmploymentType";
                worksheet.Cell(1, 8).Value = "ExperienceLeval";
                worksheet.Cell(1, 9).Value = "EducationLeval";
                worksheet.Cell(1, 10).Value = "CreatedDate";
                worksheet.Cell(1, 11).Value = "ModifiedDate";

                // Add data
                int row = 2;
                foreach (var jobModel in jobModels)
                {
                    worksheet.Cell(row, 1).Value = jobModel.JobID;
                    worksheet.Cell(row, 2).Value = jobModel.JobType;
                    worksheet.Cell(row, 3).Value = jobModel.CompanyName;
                    worksheet.Cell(row, 4).Value = jobModel.Location;
                    worksheet.Cell(row, 5).Value = jobModel.Requirements;
                    worksheet.Cell(row, 6).Value = jobModel.Salary;
                    worksheet.Cell(row, 7).Value = jobModel.EmploymentType;
                    worksheet.Cell(row, 8).Value = jobModel.ExperienceLeval;
                    worksheet.Cell(row, 9).Value = jobModel.EducationLeval;
                    worksheet.Cell(row, 10).Value = jobModel.CreatedDate.ToString("yyyy-MM-dd");
                    worksheet.Cell(row, 11).Value = jobModel.ModifiedDate.ToString("yyyy-MM-dd");


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

        #region #Cancel
        public IActionResult Cancel()
        {
            return RedirectToAction("JobList");
        }
        #endregion


    }
}
