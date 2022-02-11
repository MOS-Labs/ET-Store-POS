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
        // WHLocation
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
                Console.WriteLine($"GetWHLocationDetails Returned Rows : {dtSQLLoginResult.Rows.Count.ToString()}");

                return dtSQLLoginResult;
            }
            catch (Exception e)
            {
                MessageBox.Show("The attempt to retrieve location details for Warehouse ID: " + WHID + " might have failed. Please check with your system admin" + nl + e.Message,
                "Error ESP_WHL1: DB Read", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        public DataTable AddWHLocation(string UserID, string Password, int WHID, string Name, int StaffID)
        {
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand("AddWHLocation", myConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                myCommand.Parameters.Add("@WHID", SqlDbType.Int).Value = WHID;
                myCommand.Parameters.Add("@Name", SqlDbType.VarChar).Value = Name;
                myCommand.Parameters.Add("@StaffID", SqlDbType.Int).Value = StaffID;
                SqlDataAdapter daSQLLoginResult = new SqlDataAdapter(myCommand);
                daSQLLoginResult.Fill(dtSQLLoginResult);
                daSQLLoginResult.Dispose();
                myConnection.Close();
                Console.WriteLine($"AddWHLocation Returned Rows : {dtSQLLoginResult.Rows.Count.ToString()}");

                return dtSQLLoginResult;
            }
            catch (Exception e)
            {
                MessageBox.Show("The attempt to add location might have failed. Please 'Retrieve' again and check in 'Modify' tab to verify" + nl + e.Message,
                "Error ESP_WHL2: DB Update", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        public DataTable UpdateWHLocation(string UserID, string Password, int WHID, string name, int staffID, int status)
        {
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand("UpdateWHLocation", myConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                myCommand.Parameters.Add("@WHID", SqlDbType.Int).Value = WHID;
                myCommand.Parameters.Add("@Name", SqlDbType.VarChar).Value = name;
                myCommand.Parameters.Add("@StaffID", SqlDbType.Int).Value = staffID;
                myCommand.Parameters.Add("@Status", SqlDbType.Int).Value = status;
                SqlDataAdapter daSQLLoginResult = new SqlDataAdapter(myCommand);
                daSQLLoginResult.Fill(dtSQLLoginResult);
                daSQLLoginResult.Dispose();
                myConnection.Close();
                Console.WriteLine($"UpdateWHLocation Returned Rows : {dtSQLLoginResult.Rows.Count.ToString()}");
                return dtSQLLoginResult;
            }
            catch (Exception e)
            {
                MessageBox.Show("The update to database might have failed. Please 'Retrieve' again verify" + nl + e.Message,
                "Error ESP_WHL3: DB Update", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        // WarehouseInfo
        public DataTable GetWarehouseInfo(string UserID, string Password)  // Gets Details of ALL rows in WarehouseInfo table
        {
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand("GetWarehouseInfo", myConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                SqlDataAdapter daSQLLoginResult = new SqlDataAdapter(myCommand);
                daSQLLoginResult.Fill(dtSQLLoginResult);
                daSQLLoginResult.Dispose();
                myConnection.Close();
                Console.WriteLine($"GetWarehouseInfo Returned Rows : {dtSQLLoginResult.Rows.Count.ToString()}");

                return dtSQLLoginResult;
            }
            catch (Exception e)
            {
                MessageBox.Show("The attempt to retrieve Warehouse details might have failed. Please retry. If issue persists, then check with your system admin" + nl + e.Message,
                "Error ESP_WH1: DB Read", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        public DataTable AddWarehouse(string UserID, string Password, string name, string address, string state, string city, 
                                    string PINCode, int inchargeID, string TIN, string GST, int createdByID)
        {
            bool success = true;
            DataTable dtSQLResult = new DataTable();
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand("AddWarehouse", myConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                myCommand.Parameters.Add("@Name", SqlDbType.VarChar).Value = name;
                myCommand.Parameters.Add("@Address", SqlDbType.VarChar).Value = address;
                myCommand.Parameters.Add("@State", SqlDbType.VarChar).Value = state;
                myCommand.Parameters.Add("@City", SqlDbType.VarChar).Value = city;
                myCommand.Parameters.Add("@PINCode", SqlDbType.VarChar).Value = PINCode;
                myCommand.Parameters.Add("@InchargeID", SqlDbType.Int).Value = inchargeID;
                myCommand.Parameters.Add("@TIN", SqlDbType.VarChar).Value = TIN;
                myCommand.Parameters.Add("@GST", SqlDbType.VarChar).Value = GST;
                myCommand.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = createdByID;
                SqlDataAdapter daSQLResult = new SqlDataAdapter(myCommand);
                daSQLResult.Fill(dtSQLLoginResult);
                daSQLResult.Dispose();
                myConnection.Close();
                Console.WriteLine($"AddWarehouse Returned Rows : {dtSQLLoginResult.Rows.Count.ToString()}");

                if (dtSQLResult.Rows.Count >= 1)
                {
                    string res = "";
                    foreach (DataRow d in dtSQLResult.Rows)
                    {
                        res = d["Return Value"].ToString();
                        int.TryParse(res, out int result);
                        MessageBox.Show("SQL returned result = " + result, "Testing: AddWarehouse", MessageBoxButton.OK);
                        if (result != 0)
                        {
                            MessageBox.Show("The attempt to add Warehouse did not return a confirmation. Please search in 'Modify' tab to verify" + nl,
                            "Error ESP_WH4: DB Insert Result: " + result , MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("# Rows = 0");
                    // TEMPORARILY DISABLING THE BELOW ERROR MSG ESP_WH8 BCOZ ITS ALWAYS SHOWING UP EVEN THO DB UPDATE WAS SUCCESSFUL
                    //MessageBox.Show("The attempt to add Warehouse might have failed. Please search in 'Modify' tab to verify" + nl,
                    //"Error ESP_WH5: DB Insert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return dtSQLLoginResult;
            }
            catch (Exception e)
            {
                MessageBox.Show("The attempt to add Warehouse caused an exception. Please search in 'Modify' tab to verify" + nl + e.Message,
                "Error ESP_WH6: DB Insert", MessageBoxButton.OK, MessageBoxImage.Error);
                success = false;
                throw;
            }
            // Show success message if no exception was thrown i.e catch block was not entered
            if (success) { MessageBox.Show("Warehouse added successfully", "ESP_Success", MessageBoxButton.OK, MessageBoxImage.Hand); }

        }
        public DataTable UpdateWarehouse(string UserID, string Password, int WHID, string name, string address, string state, string city,
                                    string PINCode, int inchargeID, string TIN, string GST, int lastModifiedByID, int isDeleted, int status)
        {
            DataTable dtSQLResult = new DataTable();
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand("UpdateWarehouse", myConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                myCommand.Parameters.Add("@WHID", SqlDbType.Int).Value = WHID;
                myCommand.Parameters.Add("@Name", SqlDbType.VarChar).Value = name;
                myCommand.Parameters.Add("@Address", SqlDbType.VarChar).Value = address;
                myCommand.Parameters.Add("@State", SqlDbType.VarChar).Value = state;
                myCommand.Parameters.Add("@City", SqlDbType.VarChar).Value = city;
                
                myCommand.Parameters.Add("@PINCode", SqlDbType.VarChar).Value = PINCode;
                myCommand.Parameters.Add("@InchargeID", SqlDbType.Int).Value = inchargeID;
                myCommand.Parameters.Add("@TIN", SqlDbType.VarChar).Value = TIN;
                myCommand.Parameters.Add("@GST", SqlDbType.VarChar).Value = GST;
                myCommand.Parameters.Add("@LastModifiedBy", SqlDbType.Int).Value = lastModifiedByID;
                
                myCommand.Parameters.Add("@IsDeleted", SqlDbType.Int).Value = isDeleted;
                myCommand.Parameters.Add("@Status", SqlDbType.Int).Value = status;
                SqlDataAdapter daSQLResult = new SqlDataAdapter(myCommand);
                daSQLResult.Fill(dtSQLResult);
                Console.WriteLine($"UpdateWarehouse returned Rows : {dtSQLResult.Rows.Count.ToString()}");

                if (dtSQLResult.Rows.Count >= 1)
                {
                    string res = "";
                    foreach (DataRow d in dtSQLResult.Rows)
                    {
                        res = d["Return Value"].ToString();
                        int.TryParse(res, out int result);
                        MessageBox.Show("SQL returned result = " + result, "Testing: UpdateWarehouse", MessageBoxButton.OK);

                        if (result != 0)
                        {
                            MessageBox.Show("The attempt to add Warehouse might have failed. Please search in 'Modify' tab to verify" + nl,
                            "Error ESP_WH7: DB Update Result: " + result, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("# Rows = 0");
                    // TEMPORARILY DISABLING THE BELOW ERROR MSG ESP_WH8 BCOZ ITS ALWAYS SHOWING UP EVEN THO DB UPDATE WAS SUCCESSFUL
                    // MessageBox.Show("The attempt to update Warehouse might have failed. Please search in 'Modify' tab to verify" + nl,
                    // "Error ESP_WH8: DB Update", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                daSQLResult.Dispose();
                myConnection.Close();
                return dtSQLResult;
            }
            catch (Exception e)
            {
                MessageBox.Show("The attempt to update Warehouse might have failed. Please search in 'Modify' tab to verify" + nl + e.Message,
                "Error ESP_WH9: DB Update", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
    }
}
