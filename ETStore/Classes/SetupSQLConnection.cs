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
                    SqlConnection myConnection = new SqlConnection("user id=aejea;server=LAPTOP-GTJ68FPT;Trusted_Connection=yes;database=ET_Stores;connection timeout=10");
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
