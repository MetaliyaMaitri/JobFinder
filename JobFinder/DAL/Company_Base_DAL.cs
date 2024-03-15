using JobFinder.Areas.Company.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;

namespace JobFinder.DAL
{
    public class Company_Base_DAL : DAL_Helper
    {
       

        #region Selectall
        public List<CompanyModel> CompanySelectAll()
        {
            List<CompanyModel> list = new List<CompanyModel>();
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("PR_Company_SelectAll");
            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    CompanyModel model = new CompanyModel();

                    model.CompanyID = Convert.ToInt32(reader["CompanyID"]);
                    model.CompanyName = reader["CompanyName"].ToString();
                    model.Size = Convert.ToInt32(reader["Size"]);
                    model.CityID = Convert.ToInt32(reader["CityID"]);
                    model.CityName = reader["CityName"].ToString();
                    model.Description = reader["Description"].ToString();
                    model.ImageUrl = reader["ImageUrl"].ToString();
                    model.ContactEmail = reader["ContactEmail"].ToString();
                    model.ContactPhone = reader["ContactPhone"].ToString();
                    model.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                    model.ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"]);
                    list.Add(model);
                }
            }
            return list;
        }
        #endregion

        #region LOC_Company_Delete
        public string DeleteCompany(int CompanyID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(constring);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("PR_Company_Delete");
                sqlDB.AddInParameter(dbCMD, "CompanyID", SqlDbType.Int, CompanyID);
                sqlDB.ExecuteNonQuery(dbCMD);
                return "Record Deleted Successfully";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion

        #region CompanySelectByID
        public CompanyModel PR_Company_SelectByPK(int CompanyID)
        {
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("PR_Company_SelectByPK");
            db.AddInParameter(cmd, "@CompanyID", SqlDbType.Int, CompanyID);
            CompanyModel model = new CompanyModel();
            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    model.CompanyID = Convert.ToInt32(reader["CompanyID"]);
                    model.CompanyName = reader["CompanyName"].ToString();
                    model.Size = Convert.ToInt32(reader["Size"]);
                    model.CityID = Convert.ToInt32(reader["CityID"]);
                  
                    model.CityName = reader["CityName"].ToString();
                    model.Description = reader["Description"].ToString();
                    model.ImageUrl = reader["ImageUrl"].ToString();
                    model.ContactEmail = reader["ContactEmail"].ToString();
                    model.ContactPhone = reader["ContactPhone"].ToString();
                    model.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                    model.ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"]);
                }
            }
            return model;
        }
        #endregion

        #region CompanyAdd
        public bool PR_Company_Insert(CompanyModel model)
        {
            try
            {
                SqlDatabase db = new SqlDatabase(constring);
                DbCommand cmd = db.GetStoredProcCommand("PR_Company_Insert");
                db.AddInParameter(cmd, "@CompanyName", SqlDbType.VarChar, model.CompanyName);
                db.AddInParameter(cmd, "@Size", SqlDbType.Int, model.Size);
                db.AddInParameter(cmd, "@CityID", SqlDbType.Int, model.CityID);
                
                db.AddInParameter(cmd, "@Description", SqlDbType.VarChar, model.Description);
                db.AddInParameter(cmd, "@ImageUrl", SqlDbType.VarChar, model.ImageUrl);
                db.AddInParameter(cmd, "@ContactEmail", SqlDbType.VarChar, model.ContactEmail);
                db.AddInParameter(cmd, "@ContactPhone", SqlDbType.VarChar, model.ContactPhone);
                db.ExecuteNonQuery(cmd);

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region CompanyUpdate
        public bool PR_Company_Update(CompanyModel model)
        {
            try
            {
                SqlDatabase db = new SqlDatabase(constring);
                DbCommand cmd = db.GetStoredProcCommand("PR_Company_Update");
                db.AddInParameter(cmd, "@CompanyID", SqlDbType.Int, model.CompanyID);
                db.AddInParameter(cmd, "@CompanyName", SqlDbType.VarChar, model.CompanyName);
                db.AddInParameter(cmd, "@Size", SqlDbType.Int, model.Size);
                db.AddInParameter(cmd, "@CityID", SqlDbType.Int, model.CityID);
                db.AddInParameter(cmd, "@Description", SqlDbType.VarChar, model.Description);
                db.AddInParameter(cmd, "@ImageUrl", SqlDbType.VarChar, model.ImageUrl);
                db.AddInParameter(cmd, "@ContactEmail", SqlDbType.VarChar, model.ContactEmail);
                db.AddInParameter(cmd, "@ContactPhone", SqlDbType.VarChar, model.ContactPhone);
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

    }
}
