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

namespace ETStore
{
    class ValidateCredentialsInSQL
    {
        public static string CredValSQL(string UserID, string Password)

        {
            try

            {

                string strValidationStatus = string.Empty;
                string strExpiryDate = string.Empty; 
                string strAccessTypeID = string.Empty; 

                DataTable dtSQLLoginResult = new DataTable();
                dtSQLLoginResult =new ExecuteStoredProcedure().GetCredentialsSPCommand(UserID, Password);

                if (dtSQLLoginResult.Rows.Count >= 1)
                {
                    DataRow drSQLLoginResult = dtSQLLoginResult.Rows[0];
                    strExpiryDate = drSQLLoginResult["ExpiryDate"].ToString();
                    strAccessTypeID = drSQLLoginResult["AccessTypeID"].ToString();
                    Console.WriteLine($"Expiry Date :{strExpiryDate}");
                    DateTime dtExpiryDate = Convert.ToDateTime(strExpiryDate);
                    Console.WriteLine(dtExpiryDate.ToShortDateString());
                    if (DateTime.Today.Date >= dtExpiryDate.Date)
                    {
                        Console.WriteLine("Proceed To Change Password Screen");
                        strValidationStatus = "PasswordExpired";
                      
                                          
                    }
                    else
                    {
                        Console.WriteLine("Proceed to Process screen");
                        if (strAccessTypeID.Equals("1"))
                        {
                            strValidationStatus = "SuperUserValidationSuccess";
                        }
                        else
                        {
                            strValidationStatus = "SuperUserValidationSuccess";
                        }
                        


                    }
                }
                else
                {
                    Console.WriteLine("Validation Failed");
                    dtSQLLoginResult =new ExecuteStoredProcedure().CheckAccountLocked(UserID);

                    if (dtSQLLoginResult.Rows.Count >= 1)
                    {
                        Console.WriteLine("Account Locked");
                        strValidationStatus = "AccountLocked";

                    }
                    else
                    {
                        Console.WriteLine("Account Active");
                        strValidationStatus = "ValidationFailed";
                    }

                    
                    
                }

                return strValidationStatus;





            }

            catch(Exception)
            {
                throw;
            }

        }

    }
     
            
            
}
