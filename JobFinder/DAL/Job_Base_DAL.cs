using JobFinder.Areas.Job.Models;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using JobFinder.Areas.State.Models;

namespace JobFinder.DAL
{
    public class Job_Base_DAL : DAL_Helper
    {
        #region Select All
        public List<JobModel> JobSelectAll()
        {
            List<JobModel> list = new List<JobModel>();
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("PR_Job_SelectAll");
            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    JobModel model = new JobModel();
                    model.JobID = Convert.ToInt32(reader["JobID"]);
                    model.JobType = reader["JobType"].ToString();
                    model.CompanyID = Convert.ToInt32(reader["CompanyID"]);
                    model.CompanyName = reader["CompanyName"].ToString();
                    model.Location = reader["Location"].ToString();
                    model.Requirements = reader["Requirements"].ToString();
                    model.Salary = reader["Salary"].ToString();
                    model.EmploymentType = reader["EmploymentType"].ToString();
                    model.ExperienceLeval = reader["ExperienceLeval"].ToString();
                    model.EducationLeval = reader["EducationLeval"].ToString();
                    model.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                    model.ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"]);

                    list.Add(model);
                }
            }
            return list;
        }
        #endregion

        #region JobSelectByID
        public JobModel PR_Job_SelectByPK(int JobID)
        {
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("PR_Job_SelectByPK");
            db.AddInParameter(cmd, "@JobID", SqlDbType.Int, JobID);

            JobModel model = new JobModel();
            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    model.JobID = Convert.ToInt32(reader["JobID"]);
                    model.JobType = reader["JobType"].ToString();
                    model.CompanyID = Convert.ToInt32(reader["CompanyID"]);
                    model.Location = reader["Location"].ToString();
                    model.Requirements = reader["Requirements"].ToString();
                    model.Salary = reader["Salary"].ToString();
                    model.EmploymentType = reader["EmploymentType"].ToString();
                    model.ExperienceLeval = reader["ExperienceLeval"].ToString();
                    model.EducationLeval = reader["EducationLeval"].ToString();
                 
                    model.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                    model.ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"]);
                }
            }
            return model;
        }
        #endregion

        #region JobAdd
        public bool PR_Job_Insert(JobModel model)

        {
            try
            {
                SqlDatabase db = new SqlDatabase(constring);
                DbCommand cmd = db.GetStoredProcCommand("PR_Job_Insert");
                db.AddInParameter(cmd, "@CompanyID", SqlDbType.Int, model.CompanyID);             
                db.AddInParameter(cmd, "@JobType", SqlDbType.VarChar, model.JobType);
                db.AddInParameter(cmd, "@Location", SqlDbType.VarChar, model.Location);
                db.AddInParameter(cmd, "@Requirements", SqlDbType.VarChar, model.Requirements);
                db.AddInParameter(cmd, "@Salary", SqlDbType.VarChar, model.Salary);
                db.AddInParameter(cmd, "@EmploymentType", SqlDbType.VarChar, model.EmploymentType);
                db.AddInParameter(cmd, "@ExperienceLeval", SqlDbType.VarChar, model.ExperienceLeval);
                db.AddInParameter(cmd, "@EducationLeval", SqlDbType.VarChar, model.EducationLeval);
                


                db.ExecuteNonQuery(cmd);

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region JobUpdate
        public bool PR_Job_Update(JobModel model)
        {
            try
            {
                SqlDatabase db = new SqlDatabase(constring);
                DbCommand cmd = db.GetStoredProcCommand("PR_Job_Update");
                db.AddInParameter(cmd, "@JobID", SqlDbType.Int, model.JobID);
                db.AddInParameter(cmd, "@CompanyID", SqlDbType.Int, model.CompanyID);
                db.AddInParameter(cmd, "@JobType", SqlDbType.VarChar, model.JobType);
                db.AddInParameter(cmd, "@Location", SqlDbType.VarChar, model.Location);
                db.AddInParameter(cmd, "@Requirements", SqlDbType.VarChar, model.Requirements);
                db.AddInParameter(cmd, "@Salary", SqlDbType.VarChar, model.Salary);
                db.AddInParameter(cmd, "@EmploymentType", SqlDbType.VarChar, model.EmploymentType);
                db.AddInParameter(cmd, "@ExperienceLeval", SqlDbType.VarChar, model.ExperienceLeval);
                db.AddInParameter(cmd, "@EducationLeval", SqlDbType.VarChar, model.EducationLeval);
               


                int noOfRows = db.ExecuteNonQuery(cmd);
                if (noOfRows > 0) { return true; }
                else { return false; }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region JobDelete
        public string JobDelete(int JobID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(constring);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("PR_Job_Delete");
                sqlDB.AddInParameter(dbCMD, "JobID", SqlDbType.Int, JobID);
                sqlDB.ExecuteNonQuery(dbCMD);
                return "Record Deleted Successfully";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion

    }
}
