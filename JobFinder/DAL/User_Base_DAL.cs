using JobFinder.Areas.Company.Models;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using JobFinder.Areas.User.Models;

namespace JobFinder.DAL
{
    public class User_Base_DAL :DAL_Helper
    {
        #region Selectall
        public List<UserModel> UserSelectAll()
        {
            List<UserModel> list = new List<UserModel>();
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("PR_User_SelectAll");
            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    UserModel model = new UserModel();

                    model.UserID = Convert.ToInt32(reader["UserID"]);
                    model.UserName = reader["UserName"].ToString();
                    model.Password = reader["Password"].ToString();
                    model.Email = reader["Email"].ToString();
                   
                    list.Add(model);
                }
            }
            return list;
        }
        #endregion
    }
}
