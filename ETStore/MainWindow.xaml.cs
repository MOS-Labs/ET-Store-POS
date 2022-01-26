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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using ETStore.Classes;

namespace ETStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

       

        
        public MainWindow()
        {
            InitializeComponent();
            
            

        }

        
        int errorID = 0;
        string strErrorMsg;
        int intAttempts = 0;

        private void BtnLogin_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {


                GetAndValidateCredentials();
            }

            catch (Exception Msg)
            {
                string strExpMsg = Msg.Message;
                MessageBox.Show(strExpMsg, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine("Bismillah");
            }
        }

        private void PwdbxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)

                try
                {


                    GetAndValidateCredentials();
                }

                catch (Exception Msg)
                {
                    string strExpMsg = Msg.Message;
                    MessageBox.Show(strExpMsg, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);

                }

        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetAndValidateCredentials();
            }

            catch (Exception Msg)
            {
                string strExpMsg = Msg.Message;
                MessageBox.Show(strExpMsg, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);

            }



        }

        public string UserIDInput()
        {
            try
            {
              string  strUserID = txtUserID.Text.ToString();
                return strUserID;

            }
            catch (Exception)
            {
                throw;
            }


        }

        public string PasswordInput()
        {
            try
            {
              string  strPassword = pwdbxPassword.Password.ToString();
                return strPassword;

            }
            catch (Exception)
            {
                throw;
            }

            

        }


        public void GetAndValidateCredentials()
        {
            try

            {
              string  strUserID = UserIDInput();
              string  strPassword = PasswordInput();
               lblErrorMessage.Content = String.Empty;
                

                errorID = CredInputValidation.CredInputVal(strUserID, strPassword);

                if (errorID == 0)
                {
                    string strValidationStatus = ValidateCredentialsInSQL.CredValSQL(strUserID, strPassword);
                    if (strValidationStatus == "PasswordExpired")
                    {
                        
                        ChangePasswordWindow CPW = new ChangePasswordWindow();
                        CPW.Show();
                        CPW.GetUserID(strUserID);
                        this.Close();
                    }
                    else if (strValidationStatus == "ValidationFailed")
                    {
                        
                        errorID = 4;
                        intAttempts += 1;
                        if (intAttempts > 3)
                        {
                            LockAccountInSQL.LockAccount(strUserID);
                            errorID = 9;
                        }
                        
                        

                    }
                    else if (strValidationStatus == "AccountLocked")
                    {
                        errorID = 9;
                    }
                    else if (strValidationStatus == "SuperUserValidationSuccess")
                    {
                        ScreenSelection SCR = new ScreenSelection();
                        SCR.Show();
                        this.Close();

                    }
                }

                if (errorID != 0)
                {
                    strErrorMsg = ExceptionMessagesList.ErrorMessages(errorID);
                    lblErrorMessage.Content = strErrorMsg.ToString();
                }
               
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}

        
