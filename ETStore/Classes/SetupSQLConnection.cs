using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ETStore
{
    class SetupSQLConnection
    {
        
        public static SqlConnection ConnectionValue
        {

        

        get
            {
                try

                {
                    string userID = System.Configuration.ConfigurationManager.AppSettings["SQLUserID"];
                    string serverID = System.Configuration.ConfigurationManager.AppSettings["SQLServer"];
                    string database = System.Configuration.ConfigurationManager.AppSettings["SQLDataBase"];
                    SqlConnection myConnection = new SqlConnection($"user id={userID};server={serverID};Trusted_Connection=yes;database={database};connection timeout=10");
                    return myConnection;
                }


                catch(Exception)
                {
                    throw;

                }
            }

           
        }
    }
}
