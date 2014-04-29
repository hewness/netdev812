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
        private Entry[] SelectedEntries;
        
        public MainWindow()
        {
            InitializeComponent();
            SelectedEntries = AddressList.ToArray();
            Display();
        }

        void Display()
        {
            Entry e = SelectedEntries[current];
            tbName.Text = e.Name;
            tbAddress.Text = e.Address;
            tbCSZ.Text = e.CSZ;
            tbPhone.Text = e.Phone;
            tbEmail.Text = e.Email;

            lblRecord.Content = "Record " + (current + 1) + " of " + SelectedEntries.Length;

            btnStart.IsEnabled = btnPrevious.IsEnabled = (current != 0);
            btnEnd.IsEnabled = btnNext.IsEnabled = (current != SelectedEntries.Length - 1);
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
            if (current < SelectedEntries.Length - 1)
                current++;
            Display();
        }

        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {
            current = SelectedEntries.Length - 1; 
            Display();
        }

        private void FindBtn_Click(object sender, RoutedEventArgs e)
        {
            if (FindBtn.Content.Equals("Reset") || String.Empty.Equals(tbFind.Text))
            {
                SelectedEntries = AddressList.ToArray();
                FindBtn.Content = "Find";
                current = 0;
            }
            else
            {
                Entry[] newSelection = (from selected in AddressList where selected.Name.ToUpper().Contains(tbFind.Text.ToUpper()) select selected).ToArray();

                if (!newSelection.Any())
                {
                    MessageBox.Show("Cannot find records that match " + tbFind.Text);
                    return;
                }
                else
                {
                    SelectedEntries = newSelection;
                    current = 0;
                }
            }
            
            Display();
        }

        private void tbFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            FindBtn.Content = SelectedEntries.Length != AddressList.Count && String.Empty.Equals(tbFind.Text) ? "Reset" : "Find";
        }
    }
}
