using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace JobFinder.DAL
{
    public class Dashboard_Base_DAL : DAL_Helper
    {
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
