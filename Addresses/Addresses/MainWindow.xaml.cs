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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Addresses
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Entries AddressList = new Entries();
        private int current = 0;
        
        public MainWindow()
        {
            InitializeComponent();
            Display();
        }

        void Display()
        {
            Entry e = AddressList[current];
            tbName.Text = e.Name;
            tbAddress.Text = e.Address;
            tbCSZ.Text = e.CSZ;
            tbPhone.Text = e.Phone;
            tbEmail.Text = e.Email;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            current = 0;
            Display();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (current > 0)
                current--;
            Display();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (current < AddressList.Count - 1)
                current++;
            Display();
        }

        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {
            current = AddressList.Count - 1; 
            Display();
        }
    }
}
