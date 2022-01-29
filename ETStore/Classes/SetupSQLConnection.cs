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
                    
                    string database = System.Configuration.ConfigurationManager.AppSettings["SQLDataBase"];

                    string serverID;

                    if (System.Environment.UserName == "aejea")
                    {
                         serverID = System.Configuration.ConfigurationManager.AppSettings["SQLServerPrimary"];

                    }
                    else
                    {
                         serverID = System.Configuration.ConfigurationManager.AppSettings["SQLServerSecondary"];
                    }

                    
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
