using JobFinder.Models;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;

namespace JobFinder.DAL
{
    public class AuthDAL : DAL_Helper
    {
        public AuthModel GetUserByEmail(string email)
        {
            AuthModel user = new AuthModel();
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("GetUserByEmail");
            db.AddInParameter(cmd, "@Email", SqlDbType.VarChar, email);

            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    user.UserId = (int)reader["userid"];
                    user.UserEmail = (string)reader["user_email"];
                    user.UserName = (string)reader["user_name"];
                    user.UserPassword = (string)reader["user_password"];
                    user.UserRole = (string)reader["user_role"];

                }
            }

            return user;
        }
        #region InsertUser
        public bool InsertUser(Register model)

        {
            try
            {
                SqlDatabase db = new SqlDatabase(constring);
                DbCommand cmd = db.GetStoredProcCommand("InsertUserClient");

                db.AddInParameter(cmd, "@UserName", SqlDbType.VarChar, model.UserName);
                db.AddInParameter(cmd, "@UserEmail", SqlDbType.VarChar, model.UserEmail);
                db.AddInParameter(cmd, "@UserPassword", SqlDbType.VarChar, model.UserPassword);
                db.AddInParameter(cmd, "@UserRole", SqlDbType.VarChar, "client");



                db.ExecuteNonQuery(cmd);

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region JobSelectByID
        public Register RegisterSelectByPk(int UserId, string UserRole)
        {
            SqlDatabase db = new SqlDatabase(constring);
            DbCommand cmd = db.GetStoredProcCommand("SelectUserByPKAndRole");
            db.AddInParameter(cmd, "@UserId", SqlDbType.Int, UserId);
            db.AddInParameter(cmd, "@user_role", SqlDbType.VarChar, UserRole);

            Register model = new Register();
            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    model.UserId = Convert.ToInt32(reader["UserId"]);
                    model.UserEmail = reader["UserEmail"].ToString();
                    model.UserName = reader["UserName"].ToString();
                    model.UserRole = reader["UserRole"].ToString();
                }
            }
            return model;
        }


        #endregion

        #region UserUpdate
        public bool UpdateUser(Register model)
        {
            try
            {
                SqlDatabase db = new SqlDatabase(constring);
                DbCommand cmd = db.GetStoredProcCommand("UpdateUser");
                db.AddInParameter(cmd, "@UserId", SqlDbType.Int, model.UserId);
                db.AddInParameter(cmd, "@UserName", SqlDbType.VarChar, model.UserName);
                db.AddInParameter(cmd, "@UserEmail", SqlDbType.VarChar, model.UserEmail);
                db.AddInParameter(cmd, "@UserPassword", SqlDbType.VarChar, model.UserPassword);


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
