using ATMSim.Models;
using ATMSim.Resources;
using iTextSharp.text;
using iTextSharp.text.pdf;
using StateManager;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace ATMSim
{
    /// <summary>
    /// Interaction logic for SessionBalancePage.xaml
    /// </summary>
    public partial class SessionBalancePage : Page
    {
        private NavigationService navigationService;
        public SessionBalancePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="navigationService">NavigationService object</param>
        public SessionBalancePage(NavigationService navigationService)
        {
            InitializeComponent();
            ATMSession.RestartSessionTimer();
            this.navigationService = navigationService;
            MainWindow mainWindow = MainWindow.Instance;
            mainWindow.Title = "ATMSim - Balance";

            DBHelper dBHelper = new DBHelper();
            double balance = dBHelper.GetBalance(ATMSession.AccountNo);

            if (balance >= 0)
            {
                BalanceTextBlock.Text = string.Concat("Rs. ", balance);
            }
            else
            {
                WpfMessageBox.Show("INTERNAL ERROR: Balance retireving failed", "ERROR", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error, WpfMessageBox.MessageBoxType.Error);
                ATMSimCommand cmd = new ATMSimCommand(CommandId.Cancel, null);
                Delegates.SetCancelBalancePage(CancelPage);
                ATMSimStateManager.AddToQueue(cmd);
            }

        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            ATMSimCommand cmd = new ATMSimCommand(CommandId.Print, null);
            Delegates.SetPrintReceipt(PrintReceipt);
            ATMSimStateManager.AddToQueue(cmd);
        }

        private void PrintReceipt(object param)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (param == null)
                {
                    ATMSession.RestartSessionTimer();
                    ShowProgressWindow();
                }
            });
        }

        private void ShowProgressWindow()
        {
            ProgressPage progressPage = new ProgressPage();
            navigationService.Navigate(progressPage);
            var progressTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(2500) };
            progressTimer.Start();
            progressTimer.Tick += (_sender, args) =>
            {
                navigationService.Navigate(this);

                progressTimer.Stop();
                progressTimer = null;
                CreateReceiptPDF();
            };
        }

        private void CreateReceiptPDF()
        {
            try
            {
                DBHelper dBHelper = new DBHelper();
                double balance = dBHelper.GetBalance(ATMSession.AccountNo);
                string pdfName = "RECEIPT" + ".pdf";
                FileStream fs = new FileStream(pdfName, FileMode.Create, FileAccess.Write, FileShare.None);
                Rectangle rec = new Rectangle(300, 400);
                Document doc = new Document(rec);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();
                doc.Add(new Paragraph("---------------------------------------------------------"));
                doc.Add(new Paragraph("ATM SIM - RECEIPT "));
                doc.Add(new Paragraph("---------------------------------------------------------"));
                doc.Add(new Paragraph("A/C :      " + ATMSession.AccountNo));
                doc.Add(new Paragraph("\nTIMESTAMP :     " + DateTime.Now.ToString()));
                doc.Add(new Paragraph("AVAIL BAL :     Rs. " + balance));
                doc.Add(new Paragraph("\n\n\n---------------------------------------------------------"));
                doc.Add(new Paragraph("THANKS FOR USING OUR ATM."));
                doc.Add(new Paragraph("CONTACT CALL CENTRE 94 77 8601499"));
                doc.Add(new Paragraph("---------------------------------------------------------"));
                doc.Close();
                ATMSession.RestartSessionTimer();
                var result = WpfMessageBox.Show("Question", "Receipt is printed as a PDF.\nDo you want to open it?", MessageBoxButton.YesNo, WpfMessageBox.MessageBoxImage.Question);
                if (result.Equals(MessageBoxResult.Yes))
                {
                    System.Diagnostics.Process.Start(@"RECEIPT.pdf");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ATMSimCommand cmd = new ATMSimCommand(CommandId.Cancel, null);
            Delegates.SetCancelBalancePage(CancelPage);
            ATMSimStateManager.AddToQueue(cmd);
        }

        private void CancelPage(object param)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (param == null)
                {
                    MainWindow.Instance.DateTimeTimer.Stop();
                    ATMSession.CancelCurrentOperation(navigationService);
                }
            });
        }

        private void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            ATMSimCommand cmd = new ATMSimCommand(CommandId.SignOut, null);
            Delegates.SetSignOutOp(SignOutOp);
            ATMSimStateManager.AddToQueue(cmd);
        }

        private void SignOutOp(object param)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (param == null)
                {
                    ATMSession.SignOut();
                }
            });
        }
    }
}
