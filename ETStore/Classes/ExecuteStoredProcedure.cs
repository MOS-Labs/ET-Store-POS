using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Windows;

namespace ETStore.Classes
{

    class ExecuteStoredProcedure
    {
        private  DataTable dtSQLLoginResult = new DataTable();
        private  SqlConnection myConnection = SetupSQLConnection.ConnectionValue;
        private static SqlConnection myStaticConnection = SetupSQLConnection.ConnectionValue;
        string nl = Environment.NewLine;
        public DataTable GetCredentialsSPCommand(string UserID, string Password)
        {
            try
           {
                
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand("GetCredentials", myConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                myCommand.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserID;
                myCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password;
                SqlDataAdapter daSQLLoginResult = new SqlDataAdapter(myCommand);
                daSQLLoginResult.Fill(dtSQLLoginResult);
                daSQLLoginResult.Dispose();
                myConnection.Close();
                Console.WriteLine($"Get Credentials Result Returned Rows : {dtSQLLoginResult.Rows.Count.ToString()}");

                return dtSQLLoginResult;
           }
            catch (Exception)
            {
                throw;
            }
        }

        public  DataTable CheckAccountLocked(string UserID)

        {
            try

            {
                
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand("CheckAccountStatus", myConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                myCommand.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserID;
                SqlDataAdapter daSQLLoginResult = new SqlDataAdapter(myCommand);
                daSQLLoginResult.Fill(dtSQLLoginResult);
                daSQLLoginResult.Dispose();
                myConnection.Close();
                Console.WriteLine($" Check Account locked SP returned rows : {dtSQLLoginResult.Rows.Count.ToString()}");
                return dtSQLLoginResult;
                               
            }

            catch (Exception)
            {
                throw;
            }

        }

        public static void LockAccount(string UserID)
        {
            try
            {

                
                myStaticConnection.Open();
                SqlCommand myCommand = new SqlCommand("LockAccount", myStaticConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                myCommand.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserID;
                myCommand.ExecuteNonQuery();
                myStaticConnection.Close();

            }
            catch (Exception)
            {
                throw;
            }

        }

        public bool ChangePassword(string UserID, string Password, DateTime ExpiryDate)
        {
            try
            {
                bool blPasswordChangeSuccess;
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

        public DataTable GetWHLocationDetails(string UserID, string Password, int WHID)
        {
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand("GetWHLocationDetails", myConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                myCommand.Parameters.Add("@WHID", SqlDbType.Int).Value = WHID;
                SqlDataAdapter daSQLLoginResult = new SqlDataAdapter(myCommand);
                daSQLLoginResult.Fill(dtSQLLoginResult);
                daSQLLoginResult.Dispose();
                myConnection.Close();
                Console.WriteLine($"Get Credentials Result Returned Rows : {dtSQLLoginResult.Rows.Count.ToString()}");

                return dtSQLLoginResult;
            }
            catch (Exception e)
            {
                MessageBox.Show("Err:ESP.GetWHLocationDetails()" + nl + e.Message);
                throw;
            }
        }
        public DataTable AddWHLocation(string UserID, string Password, int WHID)
        {
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand("GetWHLocationDetails", myConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                myCommand.Parameters.Add("@WHID", SqlDbType.Int).Value = WHID;
                SqlDataAdapter daSQLLoginResult = new SqlDataAdapter(myCommand);
                daSQLLoginResult.Fill(dtSQLLoginResult);
                daSQLLoginResult.Dispose();
                myConnection.Close();
                Console.WriteLine($"Get Credentials Result Returned Rows : {dtSQLLoginResult.Rows.Count.ToString()}");

                return dtSQLLoginResult;
            }
            catch (Exception e)
            {
                MessageBox.Show("Err:ESP.GetWHLocationDetails()" + nl + e.Message);
                throw;
            }
        }




    }
}
