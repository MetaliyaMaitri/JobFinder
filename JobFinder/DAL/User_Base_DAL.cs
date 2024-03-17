using JobFinder.Areas.User.Models;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;

namespace JobFinder.DAL
{
    public class User_Base_DAL : DAL_Helper
    {
        #region Selectall
        public List<UserModel> UserSelectAll()
        {
            List<UserModel> list = new List<UserModel>();
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("SelectAllUsers");
            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    UserModel model = new UserModel();

                    model.userid = Convert.ToInt32(reader["userid"]);
                    model.user_name = reader["user_name"].ToString();
                    model.user_email = reader["user_email"].ToString();
                    model.user_password = reader["user_password"].ToString();
                    model.user_role = reader["user_role"].ToString();

                    list.Add(model);
                }
            }
            return list;
        }
        #endregion
    }
}
