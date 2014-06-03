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
    /// Interaction logic for EmailAddressDialog.xaml
    /// </summary>
    public partial class EmailAddressDialog : Window
    {
        public string EmailAddress
        {
            get
            {
                return tbEmailAddress.Text;
            }
            set
            {
                tbEmailAddress.Text = value;
            }
        }
        
        public EmailAddressDialog()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
