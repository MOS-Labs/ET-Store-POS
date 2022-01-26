using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;

namespace ETStore.Classes
{
    class ChangePasswordInSQL
    {

        public static bool ChangePassword(string UserID,string Password,DateTime ExpiryDate)
        {
            try
            {
                bool blPasswordChangeSuccess;
                SqlConnection myConnection = SetupSQLConnection.ConnectionValue;
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand("ChangePassword", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                myCommand.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserID;
                myCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password;
                myCommand.Parameters.Add("@ExpiryDate", SqlDbType.DateTime).Value = ExpiryDate;
                myCommand.ExecuteNonQuery();
                myConnection.Close();
                
                blPasswordChangeSuccess = true;
                             
                
                return blPasswordChangeSuccess;
            }
            catch (Exception)
            {
                throw;
            }

        }


    }
}
