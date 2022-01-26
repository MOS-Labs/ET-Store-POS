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
    /// Interaction logic for ScreenSelection.xaml
    /// </summary>
    public partial class ScreenSelection : Window
    {
        public ScreenSelection()
        {
            InitializeComponent();
        }

        private void BtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            NavigateToAdmin();
            this.Close();
        }

        public static void NavigateToAdmin()
        {
            AdminWindow AW = new AdminWindow();
            AW.Show();
            
        }

        private void BtnAdmin_KeyDown(object sender, KeyEventArgs e)
        {
            AdminWindow AW = new AdminWindow();
            AW.Show();
        }
    }
}
