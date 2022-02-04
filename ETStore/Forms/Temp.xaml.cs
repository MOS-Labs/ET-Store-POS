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
    /// Interaction logic for TempShortcuts.xaml
    /// </summary>
    public partial class TempShortcuts : Window
    {
        public TempShortcuts()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbTemp.Items.Add("WHLocation");
            cmbTemp.Items.Add("Testing");
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            if (cmbTemp.Text == "WHLocation")
            {
                Forms.WHLocation whl = new Forms.WHLocation(1);
                whl.Show();
            }
            else if (cmbTemp.Text == "Testing")
            {
                Forms.Testing t = new Forms.Testing();
                t.Show();
            }
            else { }
        }
    }
}
