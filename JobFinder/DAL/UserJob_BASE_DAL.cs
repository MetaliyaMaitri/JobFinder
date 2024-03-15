using JobFinder.Areas.UserPostJob.Models;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;

namespace JobFinder.DAL
{
    public class UserJob_BASE_DAL : DAL_Helper
    {
        #region Select All
        public List<UserJobModel> JobSelectAll()
        {
            List<UserJobModel> list = new List<UserJobModel>();
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("PR_Job_SelectAll");
            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    UserJobModel model = new UserJobModel();
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
        public UserJobModel PR_Job_SelectByPK(int JobID)
        {
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("PR_Job_SelectByPK");
            db.AddInParameter(cmd, "@JobID", SqlDbType.Int, JobID);

            UserJobModel model = new UserJobModel();
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
        public bool PR_Job_Insert(UserJobModel model)

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
        public bool PR_Job_Update(UserJobModel model)
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

        #region SetCompanyDropDownList
        public DataTable Set_Company_DropDownList()
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(constring);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("LOC_CompanyDropDown");
                DataTable dt = new DataTable();
                using (IDataReader dr = sqlDB.ExecuteReader(dbCMD))
                {
                    dt.Load(dr);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
