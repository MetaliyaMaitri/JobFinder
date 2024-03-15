using JobFinder.Areas.Country.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;

namespace JobFinder.DAL
{
    public class Company_Main_DAL :Company_Base_DAL
    {
        #region Set_City_DropDownList
        public DataTable Set_City_DropDownList()
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(constring);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("LOC_CityList");
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
