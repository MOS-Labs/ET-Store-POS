using ETStore.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ETStore
{
    /// <summary>
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        public ChangePasswordWindow()
        {
            InitializeComponent();
        }

        string strUserID;
        private void BtnChangePWD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateAndChangePassword();
            }

            catch (Exception Msg)
            {
                clearTextboxes();
                string strExpMsg = Msg.Message;
                MessageBox.Show(strExpMsg, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        private void BtnChangePWD_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                ValidateAndChangePassword();
            }

            catch (Exception Msg)
            {
                clearTextboxes();
                string strExpMsg = Msg.Message;
                MessageBox.Show(strExpMsg, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        public void GetUserID(string UserID)
        {
            try
            {
              strUserID = UserID;
              Console.WriteLine(strUserID + " success");
              

            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public string OldPassword()
        {
            try
            {
              string  strOldPassword = pwdOldPWD.Password.ToString();
              return strOldPassword;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public string NewPassword()
        {
            try
            {
             string   strNewPassword = pwdNewPWD.Password.ToString();
             return strNewPassword;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public string ConfirmPassword()
        {
            try
            {
             string   strConfirmPassword = pwdConfirmPWD.Password.ToString();
             return strConfirmPassword;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public void ValidateAndChangePassword()
        {
            try
            {
                int errorID = 0;
                string strOldPassword = OldPassword();
                string strNewPassword = NewPassword();
                string strConfirmPassword = ConfirmPassword();
                DateTime dtExpiryDate = DateTime.Today.AddDays(30);

                if (String.IsNullOrEmpty(strOldPassword) || String.IsNullOrEmpty(strNewPassword) || String.IsNullOrEmpty(strConfirmPassword))
                {
                    errorID = 8;
                }
                else if (strNewPassword != strConfirmPassword)
                {
                    errorID = 5;
                }
                else if (strOldPassword == strNewPassword)
                {
                    errorID = 6;
                }
                else
                {
                    string strValidationStatus = ValidateCredentialsInSQL.CredValSQL(strUserID, strOldPassword);
                    if (strValidationStatus == "ValidationFailed")
                    {
                        errorID = 4;
                    }
                    else
                    {
                        bool blPasswordChangeSuccess =new ExecuteStoredProcedure().ChangePassword(strUserID, strNewPassword, dtExpiryDate);
                        if (blPasswordChangeSuccess)
                        {
                            Console.WriteLine("Password Changed Successfully");
                            this.Close();
                        }
                        else
                        {
                            errorID = 7;

                        }
                    }
                }
                if (errorID != 0)
                {
                    clearTextboxes();
                    string  strErrorMsg = ExceptionMessagesList.ErrorMessages(errorID);
                    lblErrorMsg.Content = strErrorMsg.ToString();
                }




            }
            catch (Exception)
            {
                throw;
            }

        }

        private void PwdConfirmPWD_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)

                try
                {
                    ValidateAndChangePassword();

                }

                catch (Exception Msg)
                {
                    clearTextboxes();
                    string strExpMsg = Msg.Message;
                    MessageBox.Show(strExpMsg, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);

                }

        }

        private void clearTextboxes()
        {
            pwdOldPWD.Clear();
            pwdNewPWD.Clear();
            pwdConfirmPWD.Clear();
        }
    }
    }
