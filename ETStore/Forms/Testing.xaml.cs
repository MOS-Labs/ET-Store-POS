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
    }
}
