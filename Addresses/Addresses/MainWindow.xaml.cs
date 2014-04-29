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
    public enum CTypes { Friend = 1, Business = 2, All = 3 };
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Entries AddressList = new Entries();
        private int current;
        private Entry[] SelectedEntries;
        private CTypes currentType;
        
        public MainWindow()
        {
            InitializeComponent();
            current = 0;
            currentType = CTypes.All;
            SelectedEntries = CreateSelectedEntries();
            Display();
        }

        private Entry[] CreateSelectedEntries()
        {
            var entries = from ent in AddressList where ((int)(ent.ContactType) & (int)(currentType)) > 0 orderby ent.Name select ent;
            return entries.ToArray();
        } 

        void Display()
        {
            Entry e = SelectedEntries[current];
            tbName.Text = e.Name;
            tbAddress.Text = e.Address;
            tbCSZ.Text = e.CSZ;
            tbPhone.Text = e.Phone;
            tbEmail.Text = e.Email;
            tbContactType.Text = e.ContactType.ToString();

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
                SelectedEntries = CreateSelectedEntries();
                FindBtn.Content = "Find";
                current = 0;
            }
            else
            {
                Entry[] newSelection = (from selected in CreateSelectedEntries() where selected.Name.ToUpper().Contains(tbFind.Text.ToUpper()) select selected).ToArray();

                if (!newSelection.Any())
                {
                    MessageBox.Show("Cannot find records that match " + tbFind.Text);
                    return;
                }

                SelectedEntries = newSelection;
                current = 0;
            }
            
            Display();
        }

        private void tbFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            FindBtn.Content = SelectedEntries.Length != AddressList.Count && String.Empty.Equals(tbFind.Text) ? "Reset" : "Find";
        }

        private void rbAll_Checked(object sender, RoutedEventArgs e)
        {
            currentType = CTypes.All;
            RefreshContactType();
        }

        private void rbFriends_Checked(object sender, RoutedEventArgs e)
        {
            currentType = CTypes.Friend;
            RefreshContactType();
        }

        private void rbBusiness_Checked(object sender, RoutedEventArgs e)
        {
            currentType = CTypes.Business;
            RefreshContactType();
        }

        private void RefreshContactType()
        {
            SelectedEntries = CreateSelectedEntries();
            current = 0;
            FindBtn.Content = "Find";
            tbFind.Text = String.Empty;
            Display();
        }
    }
}
