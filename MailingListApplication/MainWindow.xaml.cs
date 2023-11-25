using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Windows.Threading;

namespace MailingListApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DateTime lastNow;
        TimeSpan elapsedTime;
        private DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();

            //create a new dispatchtimer
            timer = new DispatcherTimer();

            //set the interval to 1 second
            timer.Interval = TimeSpan.FromSeconds(1);

            //add an event handler for the Tick event
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //compute elapsed time and dispaly
            elapsedTime += DateTime.Now - lastNow;
            txtElapsed.Text = elapsedTime.ToString(@"hh\:mm\:ss");
            lastNow = DateTime.Now;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //Start button clicked
            btnStart.IsEnabled = false;

            //disable start and exit buttons
            btnExit.IsEnabled = false;

            //enabled pause button 
            btnPause.IsEnabled = true;

            //establish start time and start timer control
            lastNow = DateTime.Now;
            timer.Start();

            //Enable mailing list frame
            grpMail.IsEnabled = true;
            txtName.Focus();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            //pause button clicked
            //disable pause button 
            btnPause.IsEnabled = false;

            //enabled start and exit buttons
            btnStart.IsEnabled = true;
            btnExit.IsEnabled = true;

            //Stop timer
            timer.Stop();

            //disable editing frame grpMail
            grpMail.IsEnabled = false;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            string boxName = ((TextBox)sender).Name;

            if(e.Key == Key.Enter)
            {
                //determine the next TextBox control to focus based on the current control
                switch (boxName)
                {
                    case "txtName":
                        txtAddress.Focus();
                        break;
                    case "txtAddress":
                        txtCity.Focus();
                        break;
                    case "txtCity":
                        txtState.Focus();
                        break;
                    case "txtZip":
                        btnAccept.Focus();
                        break;
                }
            }

            //In the zip text box, allow only nu
           if(boxName == "txtZip")
            {
                //use a regular expression to allow letters, numbers, and spaces
                if(!Regex.IsMatch(e.Key.ToString(), @"[A-Za-z0-9\s]") && e.Key != Key.Back)
                {
                    e.Handled = true;
                }
            }
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            string s;

            //accept button clicked - from label and output in messagebox
            //make sure each textbox has entry
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtAddress.Text) ||
                string.IsNullOrEmpty(txtCity.Text) || string.IsNullOrEmpty(txtState.Text) ||
                string.IsNullOrEmpty(txtZip.Text))
            {
                MessageBox.Show("Each box must have an entry!", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                txtName.Focus();
                return;
            }

            s = txtName.Text + "\r\n" + txtAddress.Text + "\r\n";
            s += txtCity.Text + ", " + txtState.Text + " " + txtZip.Text;
            MessageBox.Show(s, "Mailing Label", MessageBoxButton.OK);

            ////clear the textboxes
            //txtName.Clear();
            //txtAddress.Clear();
            //txtCity.Clear();
            //txtState.Clear();
            //txtZip.Clear();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            //clear the textboxes
            txtName.Clear();
            txtAddress.Clear();
            txtCity.Clear();
            txtState.Clear();
            txtZip.Clear();
        }
    }
}
