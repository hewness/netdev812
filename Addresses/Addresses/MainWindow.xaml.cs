using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using MessageBox = System.Windows.MessageBox;

namespace Addresses
{
    public enum CTypes { Friend, Business, All };
    public enum PTypes { Home, Mobile, Business };
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AddressesEntities DB = new AddressesEntities();
        private int _current;
        
        private Entry[] _selectedEntries;
        private CTypes _currentType;
        
        private Email[] _selectedEmails;
        private int _curEmail;

        private Phone[] _selectedPhones;
        private int _currentPhoneNumber;
        
        public MainWindow()
        {
            InitializeComponent();

            PhoneTypeComboBox.Items.Add("Business");
            PhoneTypeComboBox.Items.Add("Home");
            PhoneTypeComboBox.Items.Add("Mobile");

            _current = 0;
            _curEmail = 0;
            _currentPhoneNumber = 0;
            _currentType = CTypes.All;
            _selectedEntries = CreateSelectedEntries();
            
            Display();
        }

        private Entry[] CreateSelectedEntries()
        {
            var entries = from ent in DB.Entries where (_currentType == CTypes.All || (CTypes)(ent.ContactType) == _currentType) orderby ent.Name select ent;
            return entries.ToArray();
        } 

        void Display()
        {
            if (_selectedEntries.Length > 0)
            {
                Entry e = _selectedEntries[_current];
                tbName.Text = e.Name;
                tbAddress.Text = e.Address;
                tbCSZ.Text = e.CSZ;

                _selectedPhones = e.Phones.ToArray();

                if (_selectedPhones.Length > 0)
                {
                    Phone phone = _selectedPhones[_currentPhoneNumber];
                    tbPhone.Text = phone.PhoneNumber;
                    
                    if (PhoneTypeComboBox != null)
                        PhoneTypeComboBox.SelectedIndex = phone.PhoneType;
                }
                else
                {
                    tbPhone.Text = String.Empty;
                }
                
                _selectedEmails = e.Emails.ToArray();
                tbEmail.Text = _selectedEmails.Length > 0 ? _selectedEmails[_curEmail].EmailAddress : String.Empty;

                tbContactType.Text = e.ContactType.ToString();
                lblRecord.Content = "Record " + (_current + 1) + " of " + _selectedEntries.Length;
            }
            else
            {
                tbName.Text = String.Empty;
                tbAddress.Text = String.Empty;
                tbCSZ.Text = String.Empty;
                tbPhone.Text = String.Empty;
                _selectedEmails = new Email[]{};
                tbEmail.Text = String.Empty;
                tbContactType.Text = String.Empty;
                lblRecord.Content = "Record 0 of " + _selectedEntries.Length;
            }

            btnStart.IsEnabled = btnPrevious.IsEnabled = (_current != 0 && _selectedEntries.Length > 0);
            btnEnd.IsEnabled = btnNext.IsEnabled = (_current != _selectedEntries.Length - 1 && _selectedEntries.Length > 0);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            _current = 0;
            _curEmail = 0;
            _currentPhoneNumber = 0;
            Display();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (_current > 0)
            {
                _current--;
                _curEmail = 0;
                _currentPhoneNumber = 0;
            }
            Display();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_current < _selectedEntries.Length - 1)
            {
                _current++;
                _curEmail = 0;
                _currentPhoneNumber = 0;
            }
            Display();
        }

        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {
            _current = _selectedEntries.Length - 1;
            _curEmail = 0;
            _currentPhoneNumber = 0;
            Display();
        }

        private void FindBtn_Click(object sender, RoutedEventArgs e)
        {
            if (FindBtn.Content.Equals("Reset") || String.Empty.Equals(tbFind.Text))
            {
                _selectedEntries = CreateSelectedEntries();
                FindBtn.Content = "Find";
                _current = 0;
                _curEmail = 0;
                _currentPhoneNumber = 0;
            }
            else
            {
                Entry[] newSelection = (from selected in CreateSelectedEntries() where selected.Name.ToUpper().Contains(tbFind.Text.ToUpper()) select selected).ToArray();

                if (!newSelection.Any())
                {
                    MessageBox.Show("Cannot find records that match " + tbFind.Text);
                    return;
                }

                _selectedEntries = newSelection;
                _current = 0;
                _curEmail = 0;
                _currentPhoneNumber = 0;
            }
            
            Display();
        }

        private void tbFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            FindBtn.Content = _selectedEntries.Length != DB.Entries.Count() && String.Empty.Equals(tbFind.Text) ? "Reset" : "Find";
        }

        private void rbAll_Checked(object sender, RoutedEventArgs e)
        {
            _currentType = CTypes.All;
            RefreshContactType();
        }

        private void rbFriends_Checked(object sender, RoutedEventArgs e)
        {
            _currentType = CTypes.Friend;
            RefreshContactType();
        }

        private void rbBusiness_Checked(object sender, RoutedEventArgs e)
        {
            _currentType = CTypes.Business;
            RefreshContactType();
        }

        private void RefreshContactType()
        {
            _selectedEntries = CreateSelectedEntries();
            _current = 0;
            _curEmail = 0;
            _currentPhoneNumber = 0;
            FindBtn.Content = "Find";
            tbFind.Text = String.Empty;
            Display();
        }

        private void btnNextEmail_Click(object sender, RoutedEventArgs e)
        {
            _curEmail++;
            if (_curEmail >= _selectedEntries[_current].Emails.Count())
                _curEmail = 0;
            Display();
        }

        private void btnAddEmail_Click(object sender, RoutedEventArgs e)
        {
            EmailAddressDialog ead = new EmailAddressDialog();

            if (ead.ShowDialog() == true)
            {
                Email rec = new Email();
                rec.EmailAddress = ead.EmailAddress;
                _selectedEntries[_current].Emails.Add(rec);
                DB.SaveChanges();
                _selectedEmails = _selectedEntries[_current].Emails.ToArray();
                _curEmail = _selectedEmails.Length - 1;
                Display();
            }
        }

        private void btnAddEntry_Click(object sender, RoutedEventArgs e)
        {
            EntryDialog entryDialog = new EntryDialog("Add")
            {
                contactTypeDropDownBox = {SelectedIndex = 0},
                phoneTypeDropDownBox = {SelectedIndex = 0}
            };

            if (entryDialog.ShowDialog() == true)
            {
                Entry entry = new Entry
                {
                    Name = entryDialog.NameTextBox.Text,
                    Address = entryDialog.addressTextBox.Text,
                    ContactType = entryDialog.contactTypeDropDownBox.SelectedIndex,
                    CSZ = entryDialog.cszTextBox.Text,
                    Emails = new[] { new Email{ EmailAddress = entryDialog.emailTextBox.Text} },
                    Phones = new[] { new Phone { PhoneNumber = entryDialog.phoneTextBox.Text, PhoneType = entryDialog.phoneTypeDropDownBox.SelectedIndex} }
                };

                DB.Entries.Add(entry);

                DB.SaveChanges();

                Display();
            }
        }

        private void btnChangeEntry_Click(object sender, RoutedEventArgs e)
        {
            EntryDialog entryDialog = new EntryDialog("Change");

            if (entryDialog.ShowDialog() == true)
            {
                Display();
            }
        }

        private void btnUpdateEntry_Click(object sender, RoutedEventArgs e)
        {
            if ("Update".Equals(updateButton.Content))
            {
                updateButton.Content = "Save";
                ToggleEnabledField(true);
            }
            else
            {

                Entry entry = _selectedEntries[_current];
                entry.Name = tbName.Text;
                entry.Address = tbAddress.Text;
                entry.CSZ = tbCSZ.Text;

                DB.Entries.AddOrUpdate(entry);

                Email email = _selectedEmails[_curEmail];
                email.EmailAddress = tbEmail.Text;

                DB.Emails.AddOrUpdate(email);

                Phone phone = _selectedPhones[_currentPhoneNumber];
                phone.PhoneNumber = tbPhone.Text;
                phone.PhoneType = PhoneTypeComboBox.SelectedIndex;

                DB.Phones.AddOrUpdate(phone);

                DB.SaveChanges();

                updateButton.Content = "Update";
                
                ToggleEnabledField(false);

                Display();
            }
        }

        private void ToggleEnabledField(bool toggle)
        {
            tbName.IsEnabled = toggle;
            tbAddress.IsEnabled = toggle;
            tbCSZ.IsEnabled = toggle;
            tbPhone.IsEnabled = toggle;
            tbEmail.IsEnabled = toggle;
            PhoneTypeComboBox.IsEnabled = toggle;
        }

        private void btnDeleteEntry_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete " + _selectedEntries[_current].Name, "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.OK)
            {
                foreach(Phone phone in _selectedEntries[_current].Phones.ToList())
                {
                    DB.Phones.Remove(phone);
                }

                foreach (Email email in _selectedEntries[_current].Emails.ToList())
                {
                    DB.Emails.Remove(email);
                }

                DB.Entries.Remove(_selectedEntries[_current]);
                DB.SaveChanges();

                _current = 0;
                _curEmail = 0;
                _selectedEntries = CreateSelectedEntries();
                
                Display();
            }
        }

        private void btnNextPhone_Click(object sender, RoutedEventArgs e)
        {
            _currentPhoneNumber++;
            if (_currentPhoneNumber >= _selectedEntries[_current].Phones.Count())
                _currentPhoneNumber = 0;

            Display();
        }

        private void btnAddPhone_Click(object sender, RoutedEventArgs e)
        {
            PhoneDialog pd = new PhoneDialog {phoneTypeComboBox = {SelectedIndex = 0}};

            if (pd.ShowDialog() == true)
            {
                Phone rec = new Phone
                {
                    PhoneNumber = pd.phoneNumberTextBox.Text,
                    PhoneType = pd.phoneTypeComboBox.SelectedIndex
                };

                _selectedEntries[_current].Phones.Add(rec);
                DB.SaveChanges();

                _selectedPhones = _selectedEntries[_current].Phones.ToArray();
                _currentPhoneNumber = _selectedPhones.Length - 1;

                Display();
            }
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            Entry ent = _selectedEntries[_current];
            AddressUtils.ClipboardUtils.CopyToClipboard(ent.Name, ent.Address, ent.CSZ);
        }

        private void btnMap_Click(object sender, RoutedEventArgs e)
        {
            Entry ent = _selectedEntries[_current];
            IUtils.Maps.Lookup(ent.Address, ent.CSZ);
        }

        private void SaveDataXMLBtn_Click(object sender, RoutedEventArgs e)
        {
            XElement xe = CreateAddressXML();
            xe.Save("Entries.xml");
        }

        private XElement CreateAddressXML()
        {
            XElement xDoc = new XElement("Addresses",
                from c in DB.Entries.ToArray()
                select new XElement("Entry",
                  new XAttribute("ID", c.Id),
                  new XAttribute("Name", c.Name),
                  new XAttribute("Address", c.Address),
                  new XAttribute("CSZ", c.CSZ),
                    from e in DB.Emails.ToArray()
                    where e.EntryId == c.Id
                    select new XElement("Email",
                      new XAttribute("RecID", e.Id),
                      new XAttribute("ID", e.EntryId),
                      new XAttribute("EmailAddress", e.EmailAddress)),
                    from p in DB.Phones.ToArray()
                    where p.EntryId == c.Id
                    select new XElement("Phone",
                      new XAttribute("PhoneID", p.Id),
                      new XAttribute("ID", p.EntryId),
                      new XAttribute("PhoneNum", p.PhoneNumber),
                      new XAttribute("Type", p.PhoneType))));
            return xDoc;
        }

        frmPrint printform = null;    // New Class Variable

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker printthread = new BackgroundWorker();
            printthread.DoWork += Print;
            printthread.ProgressChanged += PrintProgress;
            printthread.RunWorkerCompleted += PrintCompleted;
            printthread.WorkerReportsProgress = true;
            printthread.WorkerSupportsCancellation = true;

            printform = new frmPrint(printthread); // Create form
            printthread.RunWorkerAsync();  // Call DoWork()
            // Display form, if cancelled say so
            if (printform.ShowDialog() == false)
                MessageBox.Show("Printing Aborted");
        }

        private void PrintProgress(object sender, ProgressChangedEventArgs e)
        {
            printform.UpdateProgress(e.ProgressPercentage);
        }

        private void PrintCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            printform.DialogResult = (e.Cancelled == true ? false : true);
            printform.Close();
        }

        public void Print(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker thread = (BackgroundWorker)sender;
            int currentpct = 0;
            do
            {
                if (thread.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                thread.ReportProgress(currentpct);
                Thread.Sleep(250);
                currentpct += 5;
            } while (currentpct <= 100);
        }

    }
}
