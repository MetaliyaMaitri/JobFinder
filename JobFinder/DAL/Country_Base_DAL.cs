using JobFinder.Areas.Company.Models;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using JobFinder.Areas.Country.Models;
using JobFinder.Areas.Country.Models;
using System.Data.SqlClient;


namespace JobFinder.DAL
{
    public class Country_Base_DAL : DAL_Helper
    {
       
        #region Select All
        public List<CountryModel> CountrySelectAll()
        {
            
            List<CountryModel> list = new List<CountryModel>();
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("PR_LOC_Country_SelectAll");
            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    CountryModel model = new CountryModel();

                    model.CountryID = Convert.ToInt32(reader["CountryID"]);
                    model.CountryName = reader["CountryName"].ToString();                   
                    model.CountryCode = reader["CountryCode"].ToString();                  
                    model.Created = Convert.ToDateTime(reader["Created"]);
                    model.Modified = Convert.ToDateTime(reader["Modified"]);

                    list.Add(model);
                }
            }
            return list;
        }
        #endregion


        #region CoountrySelectByID
        public CountryModel PR_LOC_Country_SelectByPK(int CountryID)
        {
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("PR_LOC_Country_SelectByPK");
            db.AddInParameter(cmd, "@CountryID", SqlDbType.Int, CountryID);
            CountryModel model = new CountryModel();
            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    model.CountryID = Convert.ToInt32(reader["CountryID"]);
                    model.CountryName = reader["CountryName"].ToString();
                    model.CountryCode = reader["CountryCode"].ToString();
                    model.Created = Convert.ToDateTime(reader["Created"]);
                    model.Modified = Convert.ToDateTime(reader["Modified"]);
                }
            }
            return model;
        }
        #endregion

        #region CountryAdd
        public bool PR_LOC_Country_Insert(CountryModel model)

        {
            try
            {
                SqlDatabase db = new SqlDatabase(constring);
                DbCommand cmd = db.GetStoredProcCommand("PR_LOC_Country_Insert");
                db.AddInParameter(cmd, "@CountryName", SqlDbType.VarChar, model.CountryName);
                db.AddInParameter(cmd, "@CountryCode", SqlDbType.VarChar, model.CountryCode);


                db.ExecuteNonQuery(cmd);

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region CountryUpdate
        public bool PR_LOC_Country_Update(CountryModel model)
        {
            try
            {
                SqlDatabase db = new SqlDatabase(constring);
                DbCommand cmd = db.GetStoredProcCommand("PR_LOC_Country_Update");
                db.AddInParameter(cmd, "@CountryID", SqlDbType.Int, model.CountryID);
                db.AddInParameter(cmd, "@CountryName", SqlDbType.VarChar, model.CountryName);
                db.AddInParameter(cmd, "@CountryCode", SqlDbType.VarChar, model.CountryCode);
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

        #region LOC_Country_Delete
        public string LOC_Country_Delete(int CountryID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(constring);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("PR_LOC_Country_Delete");
                sqlDB.AddInParameter(dbCMD, "CountryID", SqlDbType.Int, CountryID);
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
