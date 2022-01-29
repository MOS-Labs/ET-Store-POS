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
    class CheckAccountLockedInSQL
    {
        public static bool CheckAccountLocked(string UserID)

        {
            try

            {

                bool blAccountLocked;
                SqlConnection myConnection = SetupSQLConnection.ConnectionValue;
                myConnection.Open();
                DataTable dtSQLLoginResult = new DataTable();
                SqlCommand myCommand = new SqlCommand("CheckAccountStatus", myConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                myCommand.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserID;
                SqlDataAdapter daSQLLoginResult = new SqlDataAdapter(myCommand);
                daSQLLoginResult.Fill(dtSQLLoginResult);
                daSQLLoginResult.Dispose();
                myConnection.Close();
                Console.WriteLine(dtSQLLoginResult.Rows.Count.ToString());

                if (dtSQLLoginResult.Rows.Count >= 1)
                {
                    Console.WriteLine("Account Locked");
                    blAccountLocked = true;

                }
                else
                {
                    Console.WriteLine("Account Active");
                    blAccountLocked = false;
                }

                return blAccountLocked;





            }

            catch (Exception)
            {
                throw;
            }

        }
    }
}
