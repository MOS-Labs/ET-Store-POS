using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System.Globalization;
using ETStore.Classes;

// THIS CLASS IS NOT USED // TO BE DELETED

namespace ETStore.Classes
{
    public class GetWHLocationDetails
    {

        public static DataTable GetWHLocationDetailsFromSQL(string UserID, string Password)
        {
            try
            {
                SqlConnection myConnection = SetupSQLConnection.ConnectionValue;
                myConnection.Open();
                DataTable dtSQLLoginResult = new DataTable();
                SqlCommand myCommand = new SqlCommand("GetWHLocationDetails", myConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                myCommand.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserID;
                myCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password;
                SqlDataAdapter daSQLLoginResult = new SqlDataAdapter(myCommand);
                daSQLLoginResult.Fill(dtSQLLoginResult);
                daSQLLoginResult.Dispose();
                myConnection.Close();
                Console.WriteLine(dtSQLLoginResult.Rows.Count.ToString());

                return dtSQLLoginResult;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
