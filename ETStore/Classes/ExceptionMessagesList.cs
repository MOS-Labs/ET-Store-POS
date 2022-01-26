using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ETStore
{
    class ExceptionMessagesList
    {
        public static string ErrorMessages(int errID)
        {
            try
            {
                string errMessage;
                Dictionary<int, string> ExceptionsList = new Dictionary<int, string>();
                ExceptionsList.Add(1, "Please Enter User ID And Password");
                ExceptionsList.Add(2, "Password is blank");
                ExceptionsList.Add(3, "User ID is blank");
                ExceptionsList.Add(4, "User ID Or Password Provided is incorrect");
                ExceptionsList.Add(5, "New Password And Confirm Password Mismatch");
                ExceptionsList.Add(6, "Old Password And New Password cannot be same");
                ExceptionsList.Add(7, "Error encountered while reseting the Password");
                ExceptionsList.Add(8, "All Tabs must be filled");
                ExceptionsList.Add(9, "Account Locked");
                errMessage = ExceptionsList[errID].ToString();
                return errMessage;
            }

            catch(Exception)
            {
                throw;
            }
        }













    }
}
