using JobFinder.Areas.State.Models;
using JobFinder.Areas.User_Index.Models;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;

namespace JobFinder.DAL
{
    public class User_Index_Base_DAL :DAL_Helper
    {
        #region Select All
        public List<User_IndexModel> StateSelectAll()
        {
            List<User_IndexModel> list = new List<User_IndexModel>();
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("PR_LOC_State_SelectAll");
            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    User_IndexModel model = new User_IndexModel();
                    model.StateID = Convert.ToInt32(reader["StateID"]);
                    //model.CountryID = Convert.ToInt32(reader["CountryID"]);
                    model.CountryName = reader["CountryName"].ToString();
                    model.StateName = reader["StateName"].ToString();
                    model.StateCode = reader["StateCode"].ToString();
                    model.Created = Convert.ToDateTime(reader["Created"]);
                    model.Modified = Convert.ToDateTime(reader["Modified"]);

                    list.Add(model);
                }
            }
            return list;
        }
        #endregion

        #region StateSelectByID
        public User_IndexModel PR_LOC_State_SelectByPK(int StateID)
        {
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("PR_LOC_State_SelectByPK");
            db.AddInParameter(cmd, "@StateID", SqlDbType.Int, StateID);

            User_IndexModel model = new User_IndexModel();
            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    model.StateID = Convert.ToInt32(reader["StateID"]);
                    model.CountryID = Convert.ToInt32(reader["CountryID"]);
                    //model.CountryName = reader["CountryName"].ToString();
                    model.StateName = reader["StateName"].ToString();
                    model.StateCode = reader["StateCode"].ToString();
                    model.Created = Convert.ToDateTime(reader["Created"]);
                    model.Modified = Convert.ToDateTime(reader["Modified"]);
                }
            }
            return model;
        }
        #endregion

        #region StateAdd
        public bool PR_LOC_State_Insert(User_IndexModel model)

        {
            try
            {
                SqlDatabase db = new SqlDatabase(constring);
                DbCommand cmd = db.GetStoredProcCommand("PR_LOC_State_Insert");
                db.AddInParameter(cmd, "@CountryID", SqlDbType.Int, model.CountryID);
                //db.AddInParameter(cmd, "@CountryName", SqlDbType.VarChar, model.CountryName);
                db.AddInParameter(cmd, "@StateName", SqlDbType.VarChar, model.StateName);
                db.AddInParameter(cmd, "@StateCode", SqlDbType.VarChar, model.StateCode);


                db.ExecuteNonQuery(cmd);

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region StateUpdate
        public bool PR_LOC_State_Update(User_IndexModel model)
        {
            try
            {
                SqlDatabase db = new SqlDatabase(constring);
                DbCommand cmd = db.GetStoredProcCommand("PR_LOC_State_Update");
                db.AddInParameter(cmd, "@StateID", SqlDbType.Int, model.StateID);
                db.AddInParameter(cmd, "@CountryID", SqlDbType.Int, model.CountryID);
                //db.AddInParameter(cmd, "@CountryName", SqlDbType.VarChar, model.CountryName);
                db.AddInParameter(cmd, "@StateName", SqlDbType.VarChar, model.StateName);
                db.AddInParameter(cmd, "@StateCode", SqlDbType.VarChar, model.StateCode);
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

        #region LOC_State_Delete
        public string LOC_State_Delete(int StateID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(constring);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("PR_LOC_State_Delete");
                sqlDB.AddInParameter(dbCMD, "StateID", SqlDbType.Int, StateID);
                sqlDB.ExecuteNonQuery(dbCMD);
                return "Record Deleted Successfully";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion


        public int CompanyCount()
        {
            int rowCount = 0;
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("PR_Company_SelectAll");

            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    rowCount++;
                }
            }

            // Use the rowCount variable as needed
            return rowCount;
        }
        public int JobCount()
        {
            int rowCount = 0;
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("PR_Job_SelectAll");

            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    rowCount++;
                }
            }

            // Use the rowCount variable as needed
            return rowCount;
        }

        public int CityCount()
        {
            int rowCount = 0;
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("PR_LOC_City_SelectAll");

            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    rowCount++;
                }
            }

            // Use the rowCount variable as needed
            return rowCount;
        }
        public int UserCount()
        {
            int rowCount = 0;
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("PR_User_SelectAll");

            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    rowCount++;
                }
            }

            // Use the rowCount variable as needed
            return rowCount;
        }
    }
}
