using JobFinder.Areas.City.Models;
using JobFinder.Areas.Company.Models;
using JobFinder.Areas.Job.Models;
using JobFinder.Areas.State.Models;
using JobFinder.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using JobFinder.Areas.Country.Models;

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

       
        #region #Cancel
        public IActionResult Cancel()
        {
            return RedirectToAction("JobList");
        }
        #endregion

       
    }
}
