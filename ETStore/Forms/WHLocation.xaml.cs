using ETStore.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ETStore.Forms
{
    /// <summary>
    /// Interaction logic for WHLocation.xaml
    /// </summary>
    public partial class WHLocation : Window
    {
        string nl = Environment.NewLine;
        ExecuteStoredProcedure esp;
        public WHLocation()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblFloor.Visibility = Visibility.Hidden;
            lblStorageRoom.Visibility = Visibility.Hidden;
            lblAisle.Visibility = Visibility.Hidden;
            lblShelf.Visibility = Visibility.Hidden;
            BtnAdd.IsEnabled = false;
            BtnRetrieve.IsEnabled = false;
        }


        private void txtWarehouseID_LostFocus(object sender, RoutedEventArgs e)
        {
            WHDetails wh = GetWHDetailsFromDB_Dummy(int.Parse(txtWarehouseID.Text));
            txtWarehouseName.Text = wh.WHName;
            txtWarehouseAddress.Text = wh.WHAddress;
        }


        public WHDetails GetWHDetailsFromDB_Dummy(int WHID)
        {
            if (WHID == 1)
            {
                return(new WHDetails(WHID, "Royapettah", "#1, TH Road, Ch - 5"));
            }
            else
            {
                return (new WHDetails(WHID, "No such Warehouse", "NA"));
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnRetrieve_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtWarehouseID.Text != "")
                {
                    retrieveWHLocationDetails(int.Parse(txtWarehouseID.Text));
                }
            }
            catch(Exception err)
            {
                MessageBox.Show("Error in Retrieve" + nl + err.Message);
            }
        }

        public void retrieveWHLocationDetails(int whid)
        {
            string strName, strStatus, strStatus2 = "";
            try
            {
                cmbLocations.Items.Clear();
                esp = new ExecuteStoredProcedure();
                string UserID = "Admin", Password = "Test"; // Temporarily Hardcoded
                DataTable dtSQLResult = new DataTable();
                dtSQLResult = esp.GetWHLocationDetails(UserID, Password, whid);
                MessageBox.Show("whid: " + whid + "; Rows = " + dtSQLResult.Rows.Count.ToString());
                if (dtSQLResult.Rows.Count >= 1)
                {
                    foreach(DataRow d in dtSQLResult.Rows)
                    {
                        strName = d["Name"].ToString();
                        strStatus = d["Status"].ToString();
                        if (strStatus.Equals("0")) { strStatus2 = "Active"; }
                        else if (strStatus.Equals("1")) { strStatus2 = "Inactive"; }
                        cmbLocations.Items.Add(strName + " | " + strStatus2);
                    }
                }
                else
                {
                    Console.WriteLine("# Rows = 0");
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Error in retrieveWHLocationDetails()" + nl + e.Message);
            }
        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            List<string> myList = new List<string>();
            myList.Add("abc"); myList.Add("xyz");

            bool v = isValid(txtCustomLocation.Text);
            MessageBox.Show(v.ToString());
        }

        // Currently this is returning int, but needs to be changed to string in future to accomodate for B1, B2 etc for basement
        private int getFloor(string name)
        {
            try
            {
                int floor = 0;
                string[] a = name.Split('F');
                string flr = a[1];
                int n = flr.IndexOf("SR");
                flr = flr.Substring(0, n);
                floor = int.Parse(flr);
                return floor;
            }
            catch(Exception e)
            {
                MessageBox.Show("Error in WHLocation Name Format(Floor):" + name + nl + e.Message);
                return -100;
            }
        }
        private int getStorageRoom(string name)
        {
            string text;
            int i;
            try
            {
                MessageBox.Show("name = " + name);
                int n = name.IndexOf("SR")+2; // Add two to account for the 2 chars in 'SR'
                i = name.Length - 1;
                MessageBox.Show("n = " + n + "; i = " + i + "name.len = " + name.Length);
                text = name.Substring(n, i - n);
                MessageBox.Show("text = " + text);
                text = text.Split('A')[0];
                return int.Parse(text);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error in WHLocation Name Format(StorageRoom):" + name + nl + e.Message);
                return -100;
            }
        }



        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string WHLocation = "W" + txtWarehouseID.Text + "F" + txtFloor.Text;
            WHLocation += "SR" + txtStorageRoom.Text + "A" + txtAisle.Text + "SF" + txtShelf.Text;
            MessageBox.Show(WHLocation);
        }


        // INPUT VALIDATION
        public static bool isValid(string input)
        {
            string strRegex = @"(^[0-9]{1,2}$)";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(input)) { return (true); }
            else { return (false); }
        }
        private void txtFloor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!isValid(txtFloor.Text))    {   lblFloor.Visibility = Visibility.Visible;   }
            else                            {   lblFloor.Visibility = Visibility.Hidden;    }
            updateAddButtonStatus();
        }

        private void txtStorageRoom_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!isValid(txtStorageRoom.Text)) { lblStorageRoom.Visibility = Visibility.Visible; }
            else { lblStorageRoom.Visibility = Visibility.Hidden; }
            updateAddButtonStatus();
        }

        private void txtAisle_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!isValid(txtAisle.Text))    { lblAisle.Visibility = Visibility.Visible; }
            else                            { lblAisle.Visibility = Visibility.Hidden;  }
            updateAddButtonStatus();
        }
        private void txtShelf_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!isValid(txtShelf.Text))    { lblShelf.Visibility = Visibility.Visible; }
            else                            { lblShelf.Visibility = Visibility.Hidden; }
            updateAddButtonStatus();
        }

        private void updateAddButtonStatus()
        {
            if (checkAllInputs() == true) { BtnAdd.IsEnabled = true; }
            else { BtnAdd.IsEnabled = false; }
        }

        private bool checkAllInputs()
        {
            bool b1, b2, b3, b4, allValid;
            b1 = isValid(txtFloor.Text);
            b2 = isValid(txtStorageRoom.Text);
            b3 = isValid(txtAisle.Text);
            b4 = isValid(txtShelf.Text);
            allValid = b1 && b2 && b3 && b4; // allValid = true only if all 4 are true
            return allValid;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtWarehouseID_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if(int.TryParse(txtWarehouseID.Text, out int discardResult))
                {
                    BtnRetrieve.IsEnabled = true;
                }
                else { BtnRetrieve.IsEnabled = false; }
            }
            catch(Exception err)
            {
                BtnRetrieve.IsEnabled = false;
            }
        }
    }

    // Temporary internal class to get WH Details
    public class WHDetails
    {
        int WHID;
        public string WHName, WHAddress;

        public WHDetails(int ID, string name, string address)
        {
            WHID = ID; WHName = name; WHAddress = address;
        }
    }

}


