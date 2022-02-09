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
    /// Interaction logic for WarehouseInfo.xaml
    /// </summary>
    public partial class WarehouseInfo : Window
    {
        string nl = Environment.NewLine;
        int staffID;
        ExecuteStoredProcedure esp;
        List<WHInfo> WHList;
        public WarehouseInfo(int stfID)
        {
            InitializeComponent();
            staffID = stfID;
        }

        //////////////////////// EVENTS
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RetrieveAllWarehouseInfo();
            Diag_ShowWHList();
            lblWHIDInvalid.Visibility = Visibility.Hidden;

        }
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }
        private void txtWHID2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsValid(txtWHID2.Text))
            {
                lblWHIDInvalid.Visibility = Visibility.Hidden;
                FilterByID(int.Parse(txtWHID2.Text));
            }
            else
            {
                if (txtWHID2.Text.Equals(""))   { lblWHIDInvalid.Visibility = Visibility.Hidden; }
                else                            { lblWHIDInvalid.Visibility = Visibility.Visible; }
            }
        }
        private void txtName2_TextChanged(object sender, TextChangedEventArgs e)
        {
            txt1.AppendText("txtName2.TxtChngd: " + sender.GetType() + ", " + sender.ToString() + ", " + sender.GetHashCode());
            if (!txtWHID2.Text.Equals("")) { return; } // Return if WHIID is already entered, bcoz WHID will give only 1 WH Info in list, so no need to filter
            FilterByName(txtName2.Text);
        }
        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            WHInfo w = GetWHFromWHList(lstWarehouseInfo.SelectedItem.ToString());
            LoadWHInfo(w);
            txtWHID2.IsEnabled = false;
        }
        private void btnClear1_Copy_Click(object sender, RoutedEventArgs e)
        {
            txtWHID2.IsEnabled = true;
            txtWHID2.Text = "";
            txtName2.Text = "";
            txtAddress2.Text = "";
            txtCity2.Text = "";
            txtState2.Text = "";
            txtPINCode2.Text = "";
            txtInchargeID2.Text = "";
            txtTIN2.Text = "";
            txtGST2.Text = "";
            lstWarehouseInfo.Items.Clear();
            addAllWHToListBox();
        }

        private void btnCancel1_Click(object sender, RoutedEventArgs e) { this.Close(); }


        public void Diag_ShowWHList()
        {
            foreach (WHInfo w in WHList)
            {
                txt1.AppendText(nl + w.ToString());
            }
        }

        private void Search()
        {
        }

        private void FilterByID(int ID)
        {
            lstWarehouseInfo.Items.Clear();
            foreach(WHInfo wh in WHList)
            {
                if(wh.ID == ID) { addWHToListBox(wh); }
            }
        }

        private void FilterByName(string searchName)
        {
            lstWarehouseInfo.Items.Clear();
            foreach (WHInfo wh in WHList)
            {
                // if (wh.name.Contains(searchName)) { addWHToListBox(wh); }
                if (wh.name.ToUpper().Contains(searchName.ToUpper())) { addWHToListBox(wh); }
            }
        }

        private WHInfo GetWHFromWHList(string s)
        {
            try
            {
                s = s.Split('|')[0];
                s = s.Trim();
                int id = int.Parse(s);
                foreach (WHInfo w in WHList)
                {
                    if(w.ID == id) { return w; }
                }
                return null;
            }
            catch(Exception e)
            {
                MessageBox.Show("Unable to load details of this Warehouse" + nl
                + e.Message + nl + e.StackTrace + nl + e.Data + nl + e.Source,
                "Error WH10: Unable to load", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private void LoadWHInfo(WHInfo w)
        {
            txtWHID2.Text = w.ID.ToString();
            txtName2.Text = w.name;
            txtAddress2.Text = w.address;
            txtCity2.Text = w.city;
            txtState2.Text = w.state;
            txtPINCode2.Text = w.PINCode;
            txtInchargeID2.Text = w.inchargeID.ToString();
            txtTIN2.Text = w.TIN;
            txtGST2.Text = w.GST;
            cmbStatus2.Items.Add("Active"); cmbStatus2.Items.Add("Inactive");
            cmbStatus2.Text = w.status;
        }

        /// CALLS TO EXECUTE STORED PROCEDURE
        public void RetrieveAllWarehouseInfo()
        {
            WHInfo wh;
            try
            {
                WHList = new List<WHInfo>();
                lstWarehouseInfo.Items.Clear();
                esp = new ExecuteStoredProcedure();
                string UserID = "Admin", Password = "Test"; // Temporarily Hardcoded
                DataTable dtSQLResult = new DataTable();
                dtSQLResult = esp.GetWarehouseInfo(UserID, Password);
                if (dtSQLResult.Rows.Count >= 1)
                {
                    int WHID, inchargeID, crID, lmID, isDel, delID, intStatus;
                    string name, address, state, city, PINCode, TIN, GST, crDate, lmDate, delDate, strStatus = "-";

                    foreach (DataRow d in dtSQLResult.Rows)
                    {
                        WHID = int.Parse(d["WarehouseID"].ToString());
                        name = d["Name"].ToString();
                        address = d["Address"].ToString();
                        state = d["State"].ToString();
                        city = d["City"].ToString();

                        PINCode = d["PINCode"].ToString();
                        inchargeID = int.Parse(d["WHInchargeID"].ToString());
                        TIN = d["WHTIN"].ToString();
                        GST = d["WHGST"].ToString();
                        crDate = d["CreatedDate"].ToString();

                        crID = int.Parse(d["CreationID"].ToString());
                        lmDate = d["LastModifiedDate"].ToString();
                        lmID = int.Parse(d["LastModifiedID"].ToString());
                        isDel = int.Parse(d["IsDeleted"].ToString());
                        delDate = d["DeletedDate"].ToString();

                        int.TryParse(d["DeletedID"].ToString(), out delID);

                        intStatus = int.Parse(d["Status"].ToString());
                        if (intStatus == 0) { strStatus = "Active"; }
                        else if (intStatus == 1) { strStatus = "Inactive"; }


                        wh = new WHInfo(WHID, name, address, city, state, PINCode, inchargeID, TIN, GST, crDate, crID, lmDate, lmID, isDel, delID, delDate, strStatus);
                        WHList.Add(wh);
                        addWHToListBox(wh);
                    }
                }
                else
                {
                    Console.WriteLine("# Rows = 0");
                    MessageBox.Show("No Warehouse details were retrieved" + nl,
                    "Error WH3: No Warehouse details", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("The attempt to retrieve Warehouse details might have failed" + nl
                    + e.Message + nl + e.StackTrace + nl + e.Data + nl + e.Source,
                "Error WH4: Possible error during retrieve", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //public void AddWHLocation()
        //{
        //    int whid;
        //    if (locations.Count == 0) { whid = int.Parse(txtWarehouseID.Text); }
        //    else { whid = locations[0].WHID; }
        //    string WHLocationName = "W" + whid + "F" + txtFloor.Text;
        //    WHLocationName += "SR" + txtStorageRoom.Text + "A" + txtAisle.Text + "SF" + txtShelf.Text;

        //    if (SearchInLocationList(WHLocationName))
        //    {
        //        MessageBox.Show("Unable to add \"" + WHLocationName + "\" because a location with this name already exists." + nl
        //            + "Please check 'Modify' tab to see existing locations",
        //            "Error WHL05: Duplicate Location", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    esp = new ExecuteStoredProcedure();
        //    string UserID = "Admin", Password = "Test"; // Temporarily Hardcoded
        //    DataTable dtSQLResult = new DataTable();
        //    dtSQLResult = esp.AddWHLocation(UserID, Password, int.Parse(txtWarehouseID.Text), WHLocationName, staffID);
        //    // NEED TO ADD CODE TO VERIFY WHETHER WHLOCATION WAS ADDED SUCCESSFULLY
        //}
        //public void UpdateWHLocationStatus()
        //{
        //    int statusFromChkBox;
        //    string name = cmbLocations.Text.Split('|')[0];
        //    name = name.Trim();
        //    if (chkStatus.IsChecked == true) { statusFromChkBox = 0; } // 0-Active
        //    else { statusFromChkBox = 1; } // 1-Inactive
        //    LocationDetails loc = GetLocationDetailsFromList(name);
        //    txt1.AppendText(nl + "UpdateWHLocationStatus()" + loc.WHID + ", " + name + ", " + statusFromChkBox);

        //    esp = new ExecuteStoredProcedure();
        //    string UserID = "Admin", Password = "Test"; // Temporarily Hardcoded
        //    DataTable dtSQLResult = new DataTable();
        //    dtSQLResult = esp.UpdateWHLocation(UserID, Password, loc.WHID, loc.name, staffID, statusFromChkBox);
        //    // Need to add code to verify whether SP was executed successfully
        //    // Retrieve from DB again to update status in combobox.
        //    // Reason for pulling again from DB instead of updating directly in combobox is so that user can be sure of db update if he/she sees latest status in combobox
        //    RetrieveWHLocationDetails(loc.WHID);
        //}




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
            bool b1, b2, allValid;
            b1 = IsValid(txtName1.Text);
            b2 = IsValid(txtInchargeID1.Text);
            allValid = b1 && b2; // allValid = true only if all 2 are true
            return allValid;
        }

        // UTILITIES


        private void addWHToListBox(WHInfo wh)
        {
            lstWarehouseInfo.Items.Add(wh.ID + " | " + wh.name + " | " + wh.city + " | " + wh.status);
        }

        private void addAllWHToListBox()
        {
            foreach(WHInfo wh in WHList)
            {
                lstWarehouseInfo.Items.Add(wh.ID + " | " + wh.name + " | " + wh.city + " | " + wh.status);
            }
        }

        // INNER CLASS
        public class WHInfo
        {
            public string name, address, city, state, PINCode, status, TIN, GST;
            public int ID, inchargeID, createdBy, lastModifiedBy, deletedBy, isDeleted;
            public string createdDate, lastModifiedDate, deletedDate;

            public WHInfo(int id, string nm, string addr, string cty, string st, string PIN, int icid, string tin, string gst,
                string crDate, int crID, string lmDate, int lmID, int isDel, int delID, string delDate, string stts)
            {
                ID = id; name = nm; address = addr; city = cty; state = st; PINCode = PIN; inchargeID = icid;
                createdDate = crDate; createdBy = crID; lastModifiedDate = lmDate; lastModifiedBy = lmID;
                isDeleted = isDel; deletedBy = delID; deletedDate = delDate; status = stts;
            }

            public override string ToString()
            {
                string s = "";
                s += ID + "; " + name + "; " + address + "; " + city + "; " + inchargeID + "; " + lastModifiedDate;
                return s;
            }
        }

    }
}
