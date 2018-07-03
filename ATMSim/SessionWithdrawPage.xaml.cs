using ATMSim.Controllers;
using ATMSim.Models;
using ATMSim.Resources;
using iTextSharp.text;
using iTextSharp.text.pdf;
using StateManager;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace ATMSim
{
    /// <summary>
    /// Interaction logic for SessionWithdrawPage.xaml
    /// </summary>
    public partial class SessionWithdrawPage : Page
    {
        private NavigationService navigationService;
        private Transaction transaction;
        private bool _canPrintReceipt = false;

        public SessionWithdrawPage()
        {
            InitializeComponent();
        }

        public SessionWithdrawPage(NavigationService navigationService)
        {
            InitializeComponent();
            WithdrawAmountTextBox.Focus();
            ATMSession.RestartSessionTimer();
            this.navigationService = navigationService;
            MainWindow mainWindow = MainWindow.Instance;
            mainWindow.Title = "ATMSim - Withdraw";

            // Get balance and show it on text block
            DBHelper dBHelper = new DBHelper();
            double balance = dBHelper.GetBalance(ATMSession.AccountNo);

            if (balance >= 0)
            {
                CurrentBalanceTextBlock.Text = string.Concat("Rs. ", balance);
            }
            else
            {
                WpfMessageBox.Show("INTERNAL ERROR: Balance retireving failed", "ERROR", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error, WpfMessageBox.MessageBoxType.Error);
                ATMSimCommand cmd = new ATMSimCommand(CommandId.Cancel, null);
                Delegates.SetCancelBalancePage(CancelPage);
                ATMSimStateManager.AddToQueue(cmd);
            }
            // end get balance
        }

        // Sign out
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

        // Cancel withdraw
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ATMSimCommand cmd = new ATMSimCommand(CommandId.Cancel, null);
            Delegates.SetCancelWithdrawPage(CancelPage);
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

        private void WithdrawAmountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ATMSession.RestartSessionTimer();
            e.Handled = !ValidationController.AreAllValidNumericChars(e.Text);
            base.OnPreviewTextInput(e);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ATMSession.RestartSessionTimer();
            if (!WithdrawAmountTextBox.Text.Equals(""))
            {
                WithdrawAmountTextBox.Text = WithdrawAmountTextBox.Text.Substring(0, WithdrawAmountTextBox.Text.Length - 1);
                WithdrawAmountTextBox.Focus();
                WithdrawAmountTextBox.Select(WithdrawAmountTextBox.Text.Length, 0);
            }
        }

        private void WithdrawAmountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ATMSession.RestartSessionTimer();
        }

        // Withdraw action
        #region Withdraw Action Region
        private void WithdrawButton_Click(object sender, RoutedEventArgs e)
        {
            ATMSimCommand cmd = new ATMSimCommand(CommandId.Withdraw, null);
            Delegates.SetWithdrawAction(WithdrawAction);
            ATMSimStateManager.AddToQueue(cmd);

            //LoginWindow loginWindow = new LoginWindow();
            //loginWindow = DrawLoginWindowAsVerifyPinWindow(loginWindow);
            //loginWindow.ShowDialog();
            //ATMSession.TimeOutOperation();
        }

        private void WithdrawAction(object param)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (param == null)
                {
                    ATMSession.RestartSessionTimer();

                    if (WithdrawAmountTextBox.Text == "")
                    {
                        var result = WpfMessageBox.Show("Warning", "Please enter an amount to withdraw", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Warning, WpfMessageBox.MessageBoxType.Warning);
                        if(result.Equals(MessageBoxResult.OK))
                        {
                            ATMSession.RestartSessionTimer();
                            WithdrawAmountTextBox.Focus();
                        }
                    }
                    else
                    {
                        double amount = Double.Parse(WithdrawAmountTextBox.Text);
                        Withdraw(amount);
                    }
                }
                else
                {
                    double amount = Double.Parse(param.ToString());
                    Withdraw(amount);
                }
            });
        }

        private void Withdraw(double amount)
        {
            if (amount < ATMSession.MIN_WITHDRAWAL_AMOUNT)
            {
                var result = WpfMessageBox.Show("Warning", "Minimum withdrawal amount is 100.", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Warning, WpfMessageBox.MessageBoxType.Warning);
                if (result.Equals(MessageBoxResult.OK))
                {
                    ATMSession.RestartSessionTimer();
                    WithdrawAmountTextBox.SelectAll();
                    WithdrawAmountTextBox.Focus();
                }
            }
            else if(amount % ATMSession.MULTIPLICATION_AMOUNT != 0)
            {
                var result = WpfMessageBox.Show("Warning", "Enter amount in multiplications of " + ATMSession.MULTIPLICATION_AMOUNT, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Warning, WpfMessageBox.MessageBoxType.Warning);
                if (result.Equals(MessageBoxResult.OK))
                {
                    ATMSession.RestartSessionTimer();
                    WithdrawAmountTextBox.SelectAll();
                    WithdrawAmountTextBox.Focus();
                }
            }
            else
            {
                var dResult = WpfMessageBox.Show("Receipt", "Do you want to print receipt?", MessageBoxButton.YesNo, WpfMessageBox.MessageBoxImage.Question);
                if (dResult.Equals(MessageBoxResult.Yes))
                {
                    ATMSession.RestartSessionTimer();
                    _canPrintReceipt = true;
                }
                else
                {
                    ATMSession.RestartSessionTimer();
                    _canPrintReceipt = false;
                }
                StartWithdrawing(amount);
            }
        }

        /// <summary>
        /// Start withdrawing
        /// </summary>
        private void StartWithdrawing(double amount)
        {
            ProgressPage progressPage = new ProgressPage();
            navigationService.Navigate(progressPage);
            var progressTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(2000) };
            progressTimer.Start();
            progressTimer.Tick += (_sender, args) =>
            {
                DBHelper dBHelper = new DBHelper();
                User user = dBHelper.GetUser(ATMSession.AccountNo);
                transaction = new Transaction(user.User_Id, ATMSession.AccountNo, DateTime.Now, amount);
                ReturnResult transactionResult = dBHelper.Withdraw(transaction);

                if (transactionResult == ReturnResult.Success)
                {
                    ATMSession.RestartSessionTimer();
                    if (_canPrintReceipt)
                    {
                        CreateReceiptPDF();
                    }
                    TransactionSuccessMsg();
                }
                else if (transactionResult == ReturnResult.IsEmpty)
                {
                    var result = WpfMessageBox.Show("Warning", "Your account balance is Rs. 0.00", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Warning, WpfMessageBox.MessageBoxType.Warning);
                    if (result.Equals(MessageBoxResult.OK))
                    {
                        ATMSession.RestartSessionTimer();
                        WithdrawAmountTextBox.SelectAll();
                        WithdrawAmountTextBox.Focus();
                    }
                }
                else if (transactionResult == ReturnResult.AmountIsGreater)
                {
                    var result = WpfMessageBox.Show("Warning", "Entered amount is greater than your account balance.", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Warning, WpfMessageBox.MessageBoxType.Warning);
                    if (result.Equals(MessageBoxResult.OK))
                    {
                        ATMSession.RestartSessionTimer();
                        WithdrawAmountTextBox.SelectAll();
                        WithdrawAmountTextBox.Focus();
                    }
                }
                else
                {
                    var result = WpfMessageBox.Show("Error", "Withdrwal Error. Please contact your bank.", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error, WpfMessageBox.MessageBoxType.Error);
                    if (result.Equals(MessageBoxResult.OK))
                    {
                        ATMSession.RestartSessionTimer();
                        WithdrawAmountTextBox.SelectAll();
                        WithdrawAmountTextBox.Focus();
                    }
                }

                navigationService.GoBack();
                progressTimer.Stop();
                progressTimer = null;
            };
        }

        private void TransactionSuccessMsg()
        {
            DBHelper dBHelper = new DBHelper();
            double balance = dBHelper.GetBalance(ATMSession.AccountNo);
            string transSuccessMsg = "Transaction is successfull \n" +
                                     "\nAccount No: " + ATMSession.AccountNo +
                                     "\nWithdrawal amount: Rs. " + transaction.Amount +
                                     "\nTimestamp: " + DateTime.Now.ToString() +
                                     "\nAvailable balance: Rs. " + balance +
                                     "\n\nDo you need to do another transaction?";
            var result = WpfMessageBox.Show("Information", transSuccessMsg, MessageBoxButton.YesNo, WpfMessageBox.MessageBoxImage.Information);
            if (result.Equals(MessageBoxResult.Yes))
            {
                // change state to cancel
                ATMSimCommand cmd = new ATMSimCommand(CommandId.Cancel, null);
                Delegates.SetCancelWithdrawPage(CancelPage);
                ATMSimStateManager.AddToQueue(cmd);
            }
            if (result.Equals(MessageBoxResult.No))
            {
                ATMSimCommand cmd = new ATMSimCommand(CommandId.SignOut, null);
                Delegates.SetSignOutOp(SignOutOp);
                ATMSimStateManager.AddToQueue(cmd);
            }
        }
        #endregion

        private void CreateReceiptPDF()
        {
            try
            {
                DBHelper dBHelper = new DBHelper();
                double balance = dBHelper.GetBalance(ATMSession.AccountNo);
                string pdfName = "RECEIPT" + ".pdf";
                FileStream fs = new FileStream(pdfName, FileMode.Create, FileAccess.Write, FileShare.None);
                Rectangle rec = new Rectangle(300, 488);
                Document doc = new Document(rec);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();
                doc.Add(new Paragraph("---------------------------------------------------------"));
                doc.Add(new Paragraph("ATM SIM - RECEIPT "));
                doc.Add(new Paragraph("---------------------------------------------------------"));
                doc.Add(new Paragraph("TYPE :      WITHDRAWAL"));
                doc.Add(new Paragraph("AMOUNT :       Rs." + transaction.Amount + ".00"));
                doc.Add(new Paragraph("\nA/C :       " + ATMSession.AccountNo));
                doc.Add(new Paragraph("\nTIMESTAMP :       " + DateTime.Now.ToString()));
                doc.Add(new Paragraph("AVAIL BAL :       Rs. " + balance));
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

        private void Amount200_Click(object sender, RoutedEventArgs e)
        {
            ATMSimCommand cmd = new ATMSimCommand(CommandId.Withdraw, 200);
            Delegates.SetWithdrawAction(WithdrawAction);
            ATMSimStateManager.AddToQueue(cmd);
        }

        private void Amount500_Click(object sender, RoutedEventArgs e)
        {
            ATMSimCommand cmd = new ATMSimCommand(CommandId.Withdraw, 500);
            Delegates.SetWithdrawAction(WithdrawAction);
            ATMSimStateManager.AddToQueue(cmd);
        }

        private void Amount1000_Click(object sender, RoutedEventArgs e)
        {
            ATMSimCommand cmd = new ATMSimCommand(CommandId.Withdraw, 1000);
            Delegates.SetWithdrawAction(WithdrawAction);
            ATMSimStateManager.AddToQueue(cmd);
        }

        private void Amount2000_Click(object sender, RoutedEventArgs e)
        {
            ATMSimCommand cmd = new ATMSimCommand(CommandId.Withdraw, 2000);
            Delegates.SetWithdrawAction(WithdrawAction);
            ATMSimStateManager.AddToQueue(cmd);
        }

        private void Amount5000_Click(object sender, RoutedEventArgs e)
        {
            ATMSimCommand cmd = new ATMSimCommand(CommandId.Withdraw, 5000);
            Delegates.SetWithdrawAction(WithdrawAction);
            ATMSimStateManager.AddToQueue(cmd);
        }

        private void Amount10000_Click(object sender, RoutedEventArgs e)
        {
            ATMSimCommand cmd = new ATMSimCommand(CommandId.Withdraw, 1000);
            Delegates.SetWithdrawAction(WithdrawAction);
            ATMSimStateManager.AddToQueue(cmd);
        }

        private void Amount20000_Click(object sender, RoutedEventArgs e)
        {
            ATMSimCommand cmd = new ATMSimCommand(CommandId.Withdraw, 20000);
            Delegates.SetWithdrawAction(WithdrawAction);
            ATMSimStateManager.AddToQueue(cmd);
        }

        //private LoginWindow DrawLoginWindowAsVerifyPinWindow(LoginWindow loginWindow)
        //{
        //    // texts
        //    loginWindow.Title = "Verify PIN";
        //    loginWindow.SignInButton.Content = "OK";
        //    loginWindow.PINTextBlock.Text = "Enter your PIN here";

        //    // visibilty
        //    loginWindow.AccountNoTextBlock.Visibility = Visibility.Hidden;
        //    loginWindow.AccountNoTextBox.Visibility = Visibility.Hidden;
        //    loginWindow.Image.Visibility = Visibility.Hidden;

        //    // alignment
        //    loginWindow.TextGrid.HorizontalAlignment = HorizontalAlignment.Center;
        //    loginWindow.ButtonGrid.HorizontalAlignment = HorizontalAlignment.Right;

        //    // size
        //    loginWindow.Width = 400;
        //    loginWindow.Height = 200;
        //    loginWindow.TextGrid.Margin = new Thickness(20, 30, 0, 0);
        //    loginWindow.PINTextBlock.Margin = new Thickness(0,0,0,0);
        //    loginWindow.PinPasswordBox.Margin = new Thickness(0, 20, 0, 0);
        //    loginWindow.ButtonGrid.Margin = new Thickness(0,110,20,0);

        //    return loginWindow;
        //}
    }
}
