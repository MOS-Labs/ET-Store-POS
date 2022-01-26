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

                string strValidationStatus;
                string strExpiryDate;
                string strAccessTypeID;
                /*SqlConnection myConnection = SetupSQLConnection.ConnectionValue;
                myConnection.Open();
                DataTable dtSQLLoginResult = new DataTable();
                SqlCommand myCommand = new SqlCommand("GetCredentials", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                myCommand.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserID;
                myCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password;
                SqlDataAdapter daSQLLoginResult = new SqlDataAdapter(myCommand);
                daSQLLoginResult.Fill(dtSQLLoginResult);
                daSQLLoginResult.Dispose();
                myConnection.Close();
                Console.WriteLine(dtSQLLoginResult.Rows.Count.ToString());
                */

                DataTable dtSQLLoginResult = new DataTable();
                dtSQLLoginResult = GetCredentialsSQLCommand.GetCredentialsSPCommand(UserID, Password);

                if (dtSQLLoginResult.Rows.Count >= 1)
                {
                    DataRow drSQLLoginResult = dtSQLLoginResult.Rows[0];
                    strExpiryDate = drSQLLoginResult["ExpiryDate"].ToString();
                    strAccessTypeID = drSQLLoginResult["AccessTypeID"].ToString();
                    Console.WriteLine(strExpiryDate);
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
                    bool blAccountLocked = CheckAccountLockedInSQL.CheckAccountLocked(UserID);
                    if (blAccountLocked)
                    {
                        strValidationStatus = "AccountLocked";
                    }
                    else
                    {
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
