using ETStore.Classes;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace ETStore.Forms
{
    /// <summary>
    /// Interaction logic for WHLocation.xaml
    /// </summary>
    public partial class WHLocation : Window
    {
        public WHLocation()
        {
            InitializeComponent();
        }

        private void txtWarehouseID_LostFocus(object sender, RoutedEventArgs e)
        {
            WHDetails wh = GetWHDetailsFromDB(int.Parse(txtWarehouseID.Text));
            txtWarehouseName.Text = wh.WHName;
            txtWarehouseAddress.Text = wh.WHAddress;
        }


        public WHDetails GetWHDetailsFromDB(int WHID)
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
            string WHLocation = "";
            WHLocation += "FLR" + cmbFloor.Text + "; SROOM" + cmbStorageRoom.Text + "; AIS" + cmbAisle.Text + "; ";
            MessageBox.Show(WHLocation);
        }

        private void BtnRetrieve_Click(object sender, RoutedEventArgs e)
        {

        }

        public void retrieveWHLocationDetails()
        {
            string UserID = "Admin", Password = "Test";
            DataTable dtSQLResult = new DataTable();
            dtSQLResult = GetWHLocationDetails.GetWHLocationDetailsFromSQL(UserID, Password);

            if (dtSQLResult.Rows.Count >= 1)
            {
                DataRow drSQLLoginResult = dtSQLResult.Rows[0];


            }
            else
            {
                Console.WriteLine("# Rows = 0");
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
            WHID = ID;  WHName = name;  WHAddress = address;
        }
    }
}
