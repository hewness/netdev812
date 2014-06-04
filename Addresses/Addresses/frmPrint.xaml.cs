using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for frmPrint.xaml
    /// </summary>
    public partial class frmPrint : Window
    {
        BackgroundWorker WorkerThread;     // The ‘printing’ thread
        public frmPrint(BackgroundWorker thread)
        {
            WorkerThread = thread;	     // Save so we could cancel
            InitializeComponent();
        }

        public void UpdateProgress(int amt)
        {
            pBar.Value = amt;			// Progress so far
        }

        // Add this function by double-clicking the cancel button
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            WorkerThread.CancelAsync();	// Cancel printing
        }

        
        public frmPrint()
        {
            InitializeComponent();
        }
    }
}
