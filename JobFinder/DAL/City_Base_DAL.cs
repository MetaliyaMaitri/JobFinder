
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using JobFinder.Areas.City.Models;
using JobFinder.Areas.State.Models;

namespace JobFinder.DAL
{
    public class City_Base_DAL : DAL_Helper
    {
        #region Select All
        public List<CityModel> CitySelectAll()
        {
            List<CityModel> list = new List<CityModel>();
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("PR_LOC_City_SelectAll");
            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    CityModel model = new CityModel();
                    model.CityID = Convert.ToInt32(reader["CityID"]);
                    //model.StateID = Convert.ToInt32(reader["StateID"]);
                    //model.CountryID = Convert.ToInt32(reader["CountryID"]);
                    model.CountryName = reader["CountryName"].ToString();
                    model.StateName = reader["StateName"].ToString();
                    model.CityName = reader["CityName"].ToString();
                    model.CityCode = reader["CityCode"].ToString();
                    model.CreationDate = Convert.ToDateTime(reader["CreationDate"]);
                    model.Modified = Convert.ToDateTime(reader["Modified"]);

                    list.Add(model);
                }
            }
            return list;
        }
        #endregion

        #region CitySelectByID
        public CityModel PR_LOC_City_SelectByPK(int CityID)
        {
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("PR_LOC_City_SelectByPK");
            db.AddInParameter(cmd, "@CityID", SqlDbType.Int, CityID);

            CityModel model = new CityModel();
            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    model.StateID = Convert.ToInt32(reader["StateID"]);
                    model.CountryID = Convert.ToInt32(reader["CountryID"]);
                    model.CityID = Convert.ToInt32(reader["CityID"]);
                    model.CityName = reader["CityName"].ToString();
                    //model.StateName = reader["StateName"].ToString();
                    model.CityCode = reader["CityCode"].ToString();
                    model.CreationDate = Convert.ToDateTime(reader["CreationDate"]);
                    model.Modified = Convert.ToDateTime(reader["Modified"]);
                }
            }
            return model;
        }
        #endregion

        #region CityAdd
        public bool PR_LOC_City_Insert(CityModel model)

        {
            try
            {
                SqlDatabase db = new SqlDatabase(constring);
                DbCommand cmd = db.GetStoredProcCommand("PR_LOC_City_Insert");
                db.AddInParameter(cmd, "@CountryID", SqlDbType.Int, model.CountryID);
                db.AddInParameter(cmd, "@StateID", SqlDbType.Int, model.StateID);
                db.AddInParameter(cmd, "@CityName", SqlDbType.VarChar, model.CityName);
                db.AddInParameter(cmd, "@CityCode", SqlDbType.VarChar, model.CityCode);


                db.ExecuteNonQuery(cmd);

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region CityUpdate
        public bool PR_LOC_City_Update(CityModel model)
        {
            try
            {
                SqlDatabase db = new SqlDatabase(constring);
                DbCommand cmd = db.GetStoredProcCommand("PR_LOC_City_Update");
                db.AddInParameter(cmd, "@StateID", SqlDbType.Int, model.StateID);
                db.AddInParameter(cmd, "@CountryID", SqlDbType.Int, model.CountryID);
                db.AddInParameter(cmd, "@CityID", SqlDbType.Int, model.CityID);
                db.AddInParameter(cmd, "@CityName", SqlDbType.VarChar, model.CityName);
                db.AddInParameter(cmd, "@CityCode", SqlDbType.VarChar, model.CityCode);
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

        #region LOC_City_Delete
        public string LOC_City_Delete(int CityID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(constring);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("PR_LOC_City_Delete");
                sqlDB.AddInParameter(dbCMD, "CityID", SqlDbType.Int, CityID);
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
