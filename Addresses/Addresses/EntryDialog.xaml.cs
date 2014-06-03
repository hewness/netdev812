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

namespace Addresses
{
    /// <summary>
    /// Interaction logic for EntryDialog.xaml
    /// </summary>
    public partial class EntryDialog : Window
    {
        
        public EntryDialog(String type)
        {
            InitializeComponent();

            UpdateButton.Content = type;

            contactTypeDropDownBox.Items.Add("Business");
            contactTypeDropDownBox.Items.Add("Friend");

            phoneTypeDropDownBox.Items.Add("Business");
            phoneTypeDropDownBox.Items.Add("Home");
            phoneTypeDropDownBox.Items.Add("Mobile");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
