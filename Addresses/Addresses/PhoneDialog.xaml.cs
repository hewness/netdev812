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
    /// Interaction logic for PhoneDialog.xaml
    /// </summary>
    public partial class PhoneDialog : Window
    {
        public PhoneDialog()
        {
            InitializeComponent();

            phoneTypeComboBox.Items.Add("Business");
            phoneTypeComboBox.Items.Add("Home");
            phoneTypeComboBox.Items.Add("Mobile");
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
