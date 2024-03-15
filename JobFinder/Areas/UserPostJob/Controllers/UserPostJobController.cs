using JobFinder.Areas.Company.Models;
using JobFinder.Areas.Job.Models;
using JobFinder.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using JobFinder.Areas.UserPostJob.Models;

namespace JobFinder.Areas.UserPostJob.Controllers
{
    [Area("UserPostJob")]
    [Route("UserPostJob/{controller}/{action}")]
    public class UserPostJobController : Controller
    {
        #region SelectAll

        public IActionResult UserJobList()
        {

            UserJob_BASE_DAL dal = new UserJob_BASE_DAL();
            return View(dal.JobSelectAll());

        }
        #endregion

       
        #region #SetStateDropDownList
        public void SetCompanyDropDownList()
        {
            UserJob_BASE_DAL lOC_DAL = new UserJob_BASE_DAL();
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
       
        public IActionResult UserJobAddEdit(int JobID)
        {
            SetCompanyDropDownList();
            
            if (JobID == 0)
            {
                return View();
            }
            else
            {
                UserJob_BASE_DAL dal = new UserJob_BASE_DAL();
                UserJobModel model = dal.PR_Job_SelectByPK(JobID);
                return View(model);
            }

        }
        #endregion

        #region SaveForAddEdit
        public IActionResult SaveForAddEdit(UserJobModel model)

        {
            SetCompanyDropDownList();


            bool ans = false;
            Console.WriteLine(model.JobID);
            UserJob_BASE_DAL dal = new UserJob_BASE_DAL();
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
                return RedirectToAction("UserJobAddEdit");

            }
            else
            {
                return RedirectToAction("UserJobAddEdit");
            }


        }
        #endregion


    }
}
