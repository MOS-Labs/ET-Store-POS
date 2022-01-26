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
    class LockAccountInSQL
    {
        public static void LockAccount(string UserID)
        {
            try
            {
                
                SqlConnection myConnection = SetupSQLConnection.ConnectionValue;
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand("LockAccount", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                myCommand.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserID;
                myCommand.ExecuteNonQuery();
                myConnection.Close();
                                                
            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
