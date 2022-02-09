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
        int staffID;
        ExecuteStoredProcedure esp;
        List<LocationDetails> locations;
        public WHLocation(int ID)
        {
            InitializeComponent();
            staffID = ID;
        }
        //////////////////////// EVENTS
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblFloor.Visibility = Visibility.Hidden;
            lblStorageRoom.Visibility = Visibility.Hidden;
            lblAisle.Visibility = Visibility.Hidden;
            lblShelf.Visibility = Visibility.Hidden;
            BtnAdd.IsEnabled = false;
            BtnRetrieve.IsEnabled = false;
        }
        private void BtnRetrieve_Click(object sender, RoutedEventArgs e)
        {
            WHDetails wh = GetWHDetailsFromDB_Dummy(int.Parse(txtWarehouseID.Text));
            txtWarehouseName.Text = wh.WHName;
            txtWarehouseAddress.Text = wh.WHAddress;

            int whid = 0;
            try
            {
                if (txtWarehouseID.Text != "")
                {
                    whid = int.Parse(txtWarehouseID.Text);
                    RetrieveWHLocationDetails(whid);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Unable to retrieve details for Warehouse ID: " + whid + nl + err.Message,
                "Error WHL01: Unable to retrieve", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)    {   UpdateWHLocationStatus();   }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)     {   AddWHLocation();            }
        private void BtnCancel_Click(object sender, RoutedEventArgs e) { this.Close(); }
        private void cmbLocations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbLocations.Items.Count > 0)
            {
                if ((string)cmbLocations.SelectedItem != "")
                {
                    string nameWithStatus = cmbLocations.SelectedItem.ToString();
                    string justName = nameWithStatus.Split('|')[0];
                    justName = justName.Trim();
                    int status = GetStatusFromList(justName);
                    txt1.AppendText(nl + nameWithStatus + ", " + justName + ", " + status);
                    if (status == 0) { chkStatus.IsChecked = true; }
                    else { chkStatus.IsChecked = false; }
                }
                else { chkStatus.IsChecked = false; }
            }
            else { chkStatus.IsChecked = false; }
        }
        private void txtFloor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsValid(txtFloor.Text)) { lblFloor.Visibility = Visibility.Visible; }
            else { lblFloor.Visibility = Visibility.Hidden; }
            UpdateAddButtonStatus();
        }
        private void txtStorageRoom_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsValid(txtStorageRoom.Text)) { lblStorageRoom.Visibility = Visibility.Visible; }
            else { lblStorageRoom.Visibility = Visibility.Hidden; }
            UpdateAddButtonStatus();
        }
        private void txtAisle_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsValid(txtAisle.Text)) { lblAisle.Visibility = Visibility.Visible; }
            else { lblAisle.Visibility = Visibility.Hidden; }
            UpdateAddButtonStatus();
        }
        private void txtShelf_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsValid(txtShelf.Text)) { lblShelf.Visibility = Visibility.Visible; }
            else { lblShelf.Visibility = Visibility.Hidden; }
            UpdateAddButtonStatus();
        }
        private void txtWarehouseID_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (IsValid(txtWarehouseID.Text)) { BtnRetrieve.IsEnabled = true; }
                else { BtnRetrieve.IsEnabled = false; }
            }
            catch (Exception err) { BtnRetrieve.IsEnabled = false; }
        }
        /// CALLS TO EXECUTE STORED PROCEDURE
        public void RetrieveWHLocationDetails(int whid)
        {
            try
            {
                string strName, strStatus, strStatus2 = "";
                locations = new List<LocationDetails>();
                cmbLocations.Items.Clear();
                esp = new ExecuteStoredProcedure();
                string UserID = "Admin", Password = "Test"; // Temporarily Hardcoded
                DataTable dtSQLResult = new DataTable();
                dtSQLResult = esp.GetWHLocationDetails(UserID, Password, whid);
                if (dtSQLResult.Rows.Count >= 1)
                {
                    foreach (DataRow d in dtSQLResult.Rows)
                    {
                        strName = d["Name"].ToString();
                        strStatus = d["Status"].ToString();
                        if (strStatus.Equals("0")) { strStatus2 = "Active"; }
                        else if (strStatus.Equals("1")) { strStatus2 = "Inactive"; }
                        locations.Add(new LocationDetails(whid, strName, int.Parse(strStatus)));
                        cmbLocations.Items.Add(strName + " | " + strStatus2);
                    }
                }
                else
                {
                    Console.WriteLine("# Rows = 0");
                    MessageBox.Show("No details were found for Warehouse ID: " + whid + nl,
                    "Error WHL03: No Location details", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to retrieve details for Warehouse ID: " + whid + nl 
                    + e.Message + nl + e.StackTrace + nl + e.Data + nl + e.Source,
                "Error WHL02: Unable to retrieve", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void AddWHLocation()
        {
            int whid;
            if(locations.Count == 0)   { whid = int.Parse(txtWarehouseID.Text);   }
            else                    { whid = locations[0].WHID;      }
            string WHLocationName = "W" + whid + "F" + txtFloor.Text;
            WHLocationName += "SR" + txtStorageRoom.Text + "A" + txtAisle.Text + "SF" + txtShelf.Text;

            if (SearchInLocationList(WHLocationName))
            {
                MessageBox.Show("Unable to add \"" + WHLocationName + "\" because a location with this name already exists." + nl
                    + "Please check 'Modify' tab to see existing locations",
                    "Error WHL05: Duplicate Location", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            esp = new ExecuteStoredProcedure();
            string UserID = "Admin", Password = "Test"; // Temporarily Hardcoded
            DataTable dtSQLResult = new DataTable();
            dtSQLResult = esp.AddWHLocation(UserID, Password, int.Parse(txtWarehouseID.Text), WHLocationName, staffID);
            // NEED TO ADD CODE TO VERIFY WHETHER WHLOCATION WAS ADDED SUCCESSFULLY
        }
        public void UpdateWHLocationStatus()
        {
            int statusFromChkBox;
            string name = cmbLocations.Text.Split('|')[0];
            name = name.Trim();
            if (chkStatus.IsChecked == true) { statusFromChkBox = 0; } // 0-Active
            else { statusFromChkBox = 1; } // 1-Inactive
            LocationDetails loc = GetLocationDetailsFromList(name);
            txt1.AppendText(nl + "UpdateWHLocationStatus()" + loc.WHID + ", " + name + ", " + statusFromChkBox);

            esp = new ExecuteStoredProcedure();
            string UserID = "Admin", Password = "Test"; // Temporarily Hardcoded
            DataTable dtSQLResult = new DataTable();
            dtSQLResult = esp.UpdateWHLocation(UserID, Password, loc.WHID, loc.name, staffID, statusFromChkBox);
            // Need to add code to verify whether SP was executed successfully
            // Retrieve from DB again to update status in combobox.
            // Reason for pulling again from DB instead of updating directly in combobox is so that user can be sure of db update if he/she sees latest status in combobox
            RetrieveWHLocationDetails(loc.WHID);
        }

        // INPUT VALIDATION
        public static bool IsValid(string input)
        {
            string strRegex = @"(^[0-9]{1,2}$)";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(input)) { return (true); }
            else { return (false); }
        }
        private bool checkAllInputs()
        {
            bool b1, b2, b3, b4, allValid;
            b1 = IsValid(txtFloor.Text);
            b2 = IsValid(txtStorageRoom.Text);
            b3 = IsValid(txtAisle.Text);
            b4 = IsValid(txtShelf.Text);
            allValid = b1 && b2 && b3 && b4; // allValid = true only if all 4 are true
            return allValid;
        }

        //////////////////////// UTILITIES
        private bool SearchInLocationList(string s)
        {
            foreach(LocationDetails l in locations)
            {
                if (l.name.Equals(s)) { return true; }
            }
            return false;
        }
        private int GetStatusFromList(string s)
        {
            int i = -1;
            foreach (LocationDetails l in locations)
            {
                if (l.name.Equals(s)) { i = l.status; break; }
            }
            return i;
        }
        private LocationDetails GetLocationDetailsFromList(string s)
        {
            foreach(LocationDetails l in locations)
            {
                if (l.name.Equals(s)) { return l; }
            }
            return null;
        }
        private void UpdateAddButtonStatus()
        {
            if (checkAllInputs() == true) { BtnAdd.IsEnabled = true; }
            else { BtnAdd.IsEnabled = false; }
        }
        //////////////////////// TESTING
        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            List<string> myList = new List<string>();
            myList.Add("abc"); myList.Add("xyz");

            bool v = IsValid(txtCustomLocation.Text);
            MessageBox.Show(v.ToString());
        }
        public WHDetails GetWHDetailsFromDB_Dummy(int WHID)
        {
            if (WHID == 1)
            {
                return (new WHDetails(WHID, "Royapettah", "#1, TH Road, Ch - 5"));
            }
            else
            {
                return (new WHDetails(WHID, "No such Warehouse", "NA"));
            }
        }

    }

    // Temporary internal classes to get WH details and Location details
    public class WHDetails
    {
        int WHID;
        public string WHName, WHAddress;

        public WHDetails(int ID, string name, string address)
        {
            WHID = ID; WHName = name; WHAddress = address;
        }
    }
    public class LocationDetails
    {
        public int WHID;
        public string name;
        public int status;

        public LocationDetails(int w, string n, int s)  {   WHID = w;  name = n; status = s;    }
        public override string ToString()    {   return WHID + ", " + name + ", " + status;  }
    }


}


