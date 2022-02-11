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
            lblInvalidWHID.Visibility = Visibility.Hidden;
            lblInvalidInchargeID1.Visibility = Visibility.Hidden;
            lblInvalidInchargeID2.Visibility = Visibility.Hidden;
            cmbStatus2.Items.Add("Active"); cmbStatus2.Items.Add("Inactive");
            btnSave.IsEnabled = false;
        }
        //////////////////////// EVENTS_ADD
        private void txtInchargeID1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtInchargeID1.Text.Equals("") || IsValid(txtInchargeID1.Text))
            {
                lblInvalidInchargeID1.Visibility = Visibility.Hidden;
            }
            else
            {
                lblInvalidInchargeID1.Visibility = Visibility.Visible;
                btnSave.IsEnabled = false;
            }
            checkAllInputs_Add();
        }
        private void txtName1_TextChanged(object sender, TextChangedEventArgs e) { checkAllInputs_Add(); }
        private void txtAddress1_TextChanged(object sender, TextChangedEventArgs e) { checkAllInputs_Add(); }
        private void txtCity1_TextChanged(object sender, TextChangedEventArgs e){ checkAllInputs_Add(); }
        private void txtState1_TextChanged(object sender, TextChangedEventArgs e){ checkAllInputs_Add(); }
        private void txtPINCode1_TextChanged(object sender, TextChangedEventArgs e) { checkAllInputs_Add(); }
        private void txtTIN1_TextChanged(object sender, TextChangedEventArgs e) { checkAllInputs_Add(); }
        private void txtGST1_TextChanged(object sender, TextChangedEventArgs e) { checkAllInputs_Add(); }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            AddWarehouse();
        }
        private void btnCancel1_Click(object sender, RoutedEventArgs e) { Close(); }
        //////////////////////// EVENTS_MODIFY
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            // Search might not be required if all WHInfo is loaded at window load
        }
        private void txtWHID2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsValid(txtWHID2.Text))
            {
                lblInvalidWHID.Visibility = Visibility.Hidden;
                FilterByID(int.Parse(txtWHID2.Text));
            }
            else
            {
                if (txtWHID2.Text.Equals(""))   
                {
                    lblInvalidWHID.Visibility = Visibility.Hidden;
                    lstWarehouseInfo.Items.Clear();
                    addAllWHToListBox();
                }
                else                            { lblInvalidWHID.Visibility = Visibility.Visible; }
            }
        }
        private void txtName2_TextChanged(object sender, TextChangedEventArgs e)
        {
            txt1.AppendText("txtName2.TxtChngd: " + sender.GetType() + ", " + sender.ToString() + ", " + sender.GetHashCode());
            if (!txtWHID2.Text.Equals("")) { return; } // Return if WHIID is already entered, bcoz WHID will give only 1 WH Info in list, so no need to filter
            FilterByName(txtName2.Text);
        }
        private void txtInchargeID2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtInchargeID2.Text.Equals("") || IsValid(txtInchargeID2.Text))
            {
                lblInvalidInchargeID2.Visibility = Visibility.Hidden;
            }
            else
            {
                lblInvalidInchargeID2.Visibility = Visibility.Visible;
                btnSave2.IsEnabled = false;
            }
            checkAllInputs_Modify();
        }
        private void txtAddress2_TextChanged(object sender, TextChangedEventArgs e){ checkAllInputs_Modify(); }
        private void txtCity2_TextChanged(object sender, TextChangedEventArgs e){ checkAllInputs_Modify(); }
        private void txtState2_TextChanged(object sender, TextChangedEventArgs e) { checkAllInputs_Modify(); }
        private void txtPINCode2_TextChanged(object sender, TextChangedEventArgs e) { checkAllInputs_Modify(); }
        private void txtTIN2_TextChanged(object sender, TextChangedEventArgs e) { checkAllInputs_Modify(); }
        private void txtGST2_TextChanged(object sender, TextChangedEventArgs e) { checkAllInputs_Modify(); }
        private void cmbStatus2_SelectionChanged(object sender, SelectionChangedEventArgs e) { checkAllInputs_Modify(); }
        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            WHInfo w = GetWHFromWHList(lstWarehouseInfo.SelectedItem.ToString());
            LoadWHInfo(w);
            txtWHID2.IsEnabled = false;
        }
        private void btnSave2_Click(object sender, RoutedEventArgs e)
        {
            UpdateWarehouseInfo();
            btnClear2_Click(null, null); // Calling the clear function
        }
        private void btnClear2_Click(object sender, RoutedEventArgs e)
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
            cmbStatus2.Text = "";
            lstWarehouseInfo.Items.Clear();
            lblInvalidInchargeID2.Visibility = Visibility.Hidden;
            addAllWHToListBox();
        }
        private void btnCancel2_Click(object sender, RoutedEventArgs e) { Close(); }
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
        public void UpdateWarehouseInfo()
        {
            bool success = true;
            try
            {
                int WHID, inchargeID, lmID, isDel, intStatus = -1;
                string name, address, state, city, PINCode, TIN, GST, strStatus = "-";
                WHID = int.Parse(txtWHID2.Text);
                name = txtName2.Text;
                address = txtAddress2.Text;
                state = txtState2.Text;
                city = txtCity2.Text;

                PINCode = txtPINCode2.Text;
                inchargeID = int.Parse(txtInchargeID2.Text);
                TIN = txtTIN2.Text;
                GST = txtGST2.Text;
                strStatus = cmbStatus2.Text;
                if (strStatus.Equals("Active")) { intStatus = 0; }
                else if (strStatus.Equals("Inactive")) { intStatus = 1; }

                // Temporarily Hardcoded
                string UserID = "Admin", Password = "Test"; // Temporarily Hardcoded
                lmID = 5;
                isDel = 0;
                /////////////
                esp = new ExecuteStoredProcedure();
                DataTable dtSQLResult = new DataTable();
                dtSQLResult = esp.UpdateWarehouse(UserID, Password, WHID, name, address, state, city, PINCode, inchargeID, TIN, GST, lmID, isDel, intStatus);
            }
            catch (Exception e)
            {
                MessageBox.Show("The attempt to update Warehouse details might have failed. Please load the warehouse details again to verify" + nl
                    + e.Message + nl + e.StackTrace + nl + e.Data + nl + e.Source,
                "Error WH5: DB Update", MessageBoxButton.OK, MessageBoxImage.Error);
                success = false;
            }
            // Show success message if no exception was thrown i.e catch block was not entered
            if (success) { MessageBox.Show("Warehouse details updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Hand); }
            // Retrieve from DB again to update WHList and listBox
            // Reason for pulling again from DB instead of updating directly in listBox or WHList is so that
            // user can be sure of db update if he/she sees latest status in ListBox, or if clicks on load again to see all details
            RetrieveAllWarehouseInfo();
        }
        public void AddWarehouse()
        {
            bool success = true;
            try
            {
                string UserID = "Admin", Password = "Test"; // Temporarily Hardcoded
                int createdByID = 5, inchargeID; // Temporarily Hardcoded
                string name, address, state, city, PINCode, TIN, GST;

                name = txtName1.Text;
                address = txtAddress1.Text;
                state = txtState1.Text;
                city = txtCity1.Text;
                PINCode = txtPINCode1.Text;
                inchargeID = int.Parse(txtInchargeID1.Text);
                TIN = txtTIN1.Text;
                GST = txtGST1.Text;

                esp = new ExecuteStoredProcedure();
                DataTable dtSQLResult = new DataTable();
                dtSQLResult = esp.AddWarehouse(UserID, Password, name, address, state, city, PINCode, inchargeID, TIN, GST, createdByID);
            }
            catch (Exception e)
            {
                MessageBox.Show("The attempt to add Warehouse might have failed. Please check in 'Modify' tab to verify" + nl
                    + e.Message + nl + e.StackTrace + nl + e.Data + nl + e.Source,
                "Error WH6: DB Insert", MessageBoxButton.OK, MessageBoxImage.Error);
                success = false;
            }
            // Show success message if no exception was thrown i.e catch block was not entered
            if(success) { MessageBox.Show("Warehouse added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Hand); }
            

            // Retrieve from DB again to update WHList and listBox
            // Reason for pulling again from DB instead of updating directly in listBox or WHList is so that
            // user can be sure of db update if he/she sees latest status in ListBox, or if clicks on load again to see all details
            RetrieveAllWarehouseInfo();
        }
        // INPUT VALIDATION
        public static bool IsValid(string input)
        {
            string strRegex = @"(^[0-9]{1,6}$)";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(input)) { return (true); }
            else { return (false); }
        }
        private void checkAllInputs_Add()
        {
            bool b1, b2, b3, b4, b5, b6, b7, b8, allValid;
            b1 = txtName1.Text != "";
            b2 = txtAddress1.Text != "";
            b3 = txtState1.Text != "";
            b4 = txtCity1.Text != "";
            b5 = txtPINCode1.Text != "";
            b6 = txtInchargeID1.Text!= "";
            b7 = txtTIN1.Text != "";
            b8 = txtGST1.Text != "";
            allValid = b1 && b2 && b3 && b4 && b5 && b6 && b7 && b8; // allValid = true only if all values on RHS are true
            if (allValid) { btnSave.IsEnabled = true; }
            else { btnSave.IsEnabled = false; }
        }
        private void checkAllInputs_Modify()
        {
            bool b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, allValid;
            b0 = txtWHID2.Text != "";
            b1 = txtName2.Text != "";
            b2 = txtAddress2.Text != "";
            b3 = txtState2.Text != "";
            b4 = txtCity2.Text != "";
            b5 = txtPINCode2.Text != "";
            b6 = txtInchargeID2.Text != "";
            b7 = txtTIN2.Text != "";
            b8 = txtGST2.Text != "";
            b9 = cmbStatus2.Text != "";
            allValid = b0 && b1 && b2 && b3 && b4 && b5 && b6 && b7 && b8 && b9; // allValid = true only if all values on RHS are true
            if (allValid) { btnSave2.IsEnabled = true; }
            else { btnSave2.IsEnabled = false; }
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
        private void FilterByID(int ID)
        {
            lstWarehouseInfo.Items.Clear();
            foreach (WHInfo wh in WHList)
            {
                if (wh.ID == ID) { addWHToListBox(wh); }
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
        public void Diag_ShowWHList()
        {
            foreach (WHInfo w in WHList)
            {
                txt1.AppendText(nl + w.ToString());
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
                    if (w.ID == id) { return w; }
                }
                return null;
            }
            catch (Exception e)
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
            cmbStatus2.Text = w.status;
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
                TIN = tin;  GST = gst;
                createdDate = crDate; createdBy = crID; lastModifiedDate = lmDate; lastModifiedBy = lmID;
                isDeleted = isDel; deletedBy = delID; deletedDate = delDate; status = stts;
            }
            public override string ToString()
            {
                string s = "";
                s += ID + "; " + name + "; " + address + "; " + city + "; " + inchargeID + "; " + lastModifiedDate + "; " + TIN + "; " + GST;
                return s;
            }
        }
    }
}