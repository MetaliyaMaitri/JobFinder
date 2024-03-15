using JobFinder.Areas.Country.Models;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using JobFinder.Areas.State.Models;

namespace JobFinder.DAL
{
    public class State_Base_DAL : DAL_Helper
    {
        #region Select All
        public List<StateModel> StateSelectAll()
        {
            List<StateModel> list = new List<StateModel>();
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("PR_LOC_State_SelectAll");
            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    StateModel model = new StateModel();
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
        public StateModel PR_LOC_State_SelectByPK(int StateID)
        {
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("PR_LOC_State_SelectByPK");
            db.AddInParameter(cmd, "@StateID", SqlDbType.Int, StateID);

            StateModel model = new StateModel();
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
        public bool PR_LOC_State_Insert(StateModel model)

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
        public bool PR_LOC_State_Update(StateModel model)
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


    }
}
