using System;
using System.Collections.Generic;
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
    /// Interaction logic for Testing.xaml
    /// </summary>
    public partial class Testing : Window
    {
        string nl = Environment.NewLine;
        public Testing()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbTest.Items.Add("AAA");
            cmbTest.Items.Add("BBB");
            cmbTest.Items.Add("CCC");
        }

        private void cmbTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtLog.AppendText("=================================" + nl + "cmbTest_SelectionChanged: text:" + cmbTest.Text 
                + nl + "SelectedItem: " + cmbTest.SelectedItem + nl + "SelectedItem.ToString: " + cmbTest.SelectedItem);
        }

        private void txtTest_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtLog.AppendText(nl + IsValid(txtTest.Text));
        }

        public static bool IsValid(string input)
        {
            string strRegex = @"(^[0-9]{1,3}$)";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(input)) { return (true); }
            else { return (false); }
        }

    }
}
