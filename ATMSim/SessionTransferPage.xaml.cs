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
using System.Windows.Navigation;
using System.Windows.Threading;

namespace ATMSim
{
    /// <summary>
    /// Interaction logic for SessionTransferPage.xaml
    /// </summary>
    public partial class SessionTransferPage : Page
    {
        private NavigationService _navigationService;
        private string _focusedTextBox = "account";
        private bool _canPrintReceipt = false;
        private Transaction transaction;

        public SessionTransferPage()
        {
            InitializeComponent();
        }

        public SessionTransferPage(NavigationService navigationService)
        {
            InitializeComponent();
            TransferAccountTextBox.Focus();
            ATMSession.RestartSessionTimer();
            this._navigationService = navigationService;
            MainWindow mainWindow = MainWindow.Instance;
            mainWindow.Title = "ATMSim -Transfer";

            // Get balance and show it on text block
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
            // end get balance
        }

        // Cancel operation
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ATMSimCommand cmd = new ATMSimCommand(CommandId.Cancel, null);
            Delegates.SetCancelTransferPage(CancelPage);
            ATMSimStateManager.AddToQueue(cmd);
            //ATMSession.CancelCurrentOperation(_navigationService);
        }

        private void CancelPage(object param)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (param == null)
                {
                    MainWindow.Instance.DateTimeTimer.Stop();
                    ATMSession.CancelCurrentOperation(_navigationService);
                }
            });
        }

        // Signout operation
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

        private void TransferAccountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ATMSession.RestartSessionTimer();
            e.Handled = !ValidationController.AreAllValidNumericChars(e.Text);
            base.OnPreviewTextInput(e);
        }

        private void TransferAmountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ATMSession.RestartSessionTimer();
            e.Handled = !ValidationController.AreAllValidNumericChars(e.Text);
            base.OnPreviewTextInput(e);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ATMSession.RestartSessionTimer();

            if (_focusedTextBox.Equals("account"))
            {
                ATMSession.RestartSessionTimer();
                if (!TransferAccountTextBox.Text.Equals(""))
                {
                    TransferAccountTextBox.Text = TransferAccountTextBox.Text.Substring(0, TransferAccountTextBox.Text.Length - 1);
                    TransferAccountTextBox.Focus();
                    TransferAccountTextBox.Select(TransferAccountTextBox.Text.Length, 0);
                }
            }
            if (_focusedTextBox.Equals("amount"))
            {
                ATMSession.RestartSessionTimer();
                if (!TransferAmountTextBox.Text.Equals(""))
                {
                    TransferAmountTextBox.Text = TransferAmountTextBox.Text.Substring(0, TransferAmountTextBox.Text.Length - 1);
                    TransferAmountTextBox.Focus();
                    TransferAmountTextBox.Select(TransferAmountTextBox.Text.Length, 0);
                }
            }
        }

        private void TransferAccountTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _focusedTextBox = "account";
        }

        private void TransferAmountTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _focusedTextBox = "amount";
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            ATMSimCommand cmd = new ATMSimCommand(CommandId.Transfer, null);
            Delegates.SetTransferAction(TransferAction);
            ATMSimStateManager.AddToQueue(cmd);
        }

        private void TransferAction(object param)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (param == null)
                {
                    ATMSession.RestartSessionTimer();

                    if (TransferAccountTextBox.Text == "")
                    {
                        var result = WpfMessageBox.Show("Warning", "Please enter an account to transfer", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Warning, WpfMessageBox.MessageBoxType.Warning);
                        if (result.Equals(MessageBoxResult.OK))
                        {
                            ATMSession.RestartSessionTimer();
                            TransferAccountTextBox.Focus();
                        }
                    }
                    else if (TransferAmountTextBox.Text == "")
                    {
                        var result = WpfMessageBox.Show("Warning", "Please enter an amount to transfer", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Warning, WpfMessageBox.MessageBoxType.Warning);
                        if (result.Equals(MessageBoxResult.OK))
                        {
                            ATMSession.RestartSessionTimer();
                            TransferAmountTextBox.Focus();
                        }
                    }
                    else
                    {
                        double amount = Double.Parse(TransferAmountTextBox.Text);
                        string transferToAcc = TransferAccountTextBox.Text;
                        Transfer(transferToAcc, amount);
                    }
                }
            });
        }

        private void Transfer(string transferToAcc, double amount)
        {
            if (amount < ATMSession.MIN_TRANSFER_AMOUNT)
            {
                var result = WpfMessageBox.Show("Warning", "Minimum transfer amount is" + ATMSession.MIN_TRANSFER_AMOUNT, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Warning, WpfMessageBox.MessageBoxType.Warning);
                if (result.Equals(MessageBoxResult.OK))
                {
                    ATMSession.RestartSessionTimer();
                    TransferAmountTextBox.SelectAll();
                    TransferAmountTextBox.Focus();
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
                ATMSession.RestartSessionTimer();
                StartTransfering(transferToAcc, amount);
            }
        }

        private void StartTransfering(string transferToAcc, double amount)
        {
            ProgressPage progressPage = new ProgressPage();
            _navigationService.Navigate(progressPage);
            var progressTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(2000) };
            progressTimer.Start();
            progressTimer.Tick += (_sender, args) =>
            {
                DBHelper dBHelper = new DBHelper();
                bool transferToAccIsValid = dBHelper.CheckAccountNo(transferToAcc);
                if (transferToAccIsValid)
                {
                    User user = dBHelper.GetUser(ATMSession.AccountNo);
                    transaction = new Transaction(user.User_Id, ATMSession.AccountNo, DateTime.Now, amount, transferToAcc);
                    ReturnResult transactionResult = dBHelper.Transfer(transaction);

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
                            TransferAmountTextBox.SelectAll();
                            TransferAmountTextBox.Focus();
                        }
                    }
                    else if (transactionResult == ReturnResult.AmountIsGreater)
                    {
                        var result = WpfMessageBox.Show("Warning", "Entered amount is greater than your account balance.", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Warning, WpfMessageBox.MessageBoxType.Warning);
                        if (result.Equals(MessageBoxResult.OK))
                        {
                            ATMSession.RestartSessionTimer();
                            TransferAmountTextBox.SelectAll();
                            TransferAmountTextBox.Focus();
                        }
                    }
                    else
                    {
                        var result = WpfMessageBox.Show("Error", "Transfer Error. Please contact your bank.", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error, WpfMessageBox.MessageBoxType.Error);
                        if (result.Equals(MessageBoxResult.OK))
                        {
                            ATMSession.RestartSessionTimer();
                            TransferAmountTextBox.SelectAll();
                            TransferAmountTextBox.Focus();
                        }
                    }

                    _navigationService.GoBack();
                    progressTimer.Stop();
                    progressTimer = null;
                }
                else
                {
                    var result = WpfMessageBox.Show("Error", "Transfer to account is not a valid account.", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error, WpfMessageBox.MessageBoxType.Error);
                    if (result.Equals(MessageBoxResult.OK))
                    {
                        ATMSession.RestartSessionTimer();
                        TransferAccountTextBox.SelectAll();
                        TransferAccountTextBox.Focus();
                    }

                    _navigationService.GoBack();
                    progressTimer.Stop();
                    progressTimer = null;
                }

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
                Delegates.SetCancelTransferPage(CancelPage);
                ATMSimStateManager.AddToQueue(cmd);
            }
            if (result.Equals(MessageBoxResult.No))
            {
                ATMSimCommand cmd = new ATMSimCommand(CommandId.SignOut, null);
                Delegates.SetSignOutOp(SignOutOp);
                ATMSimStateManager.AddToQueue(cmd);
            }
        }

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
                doc.Add(new Paragraph("TYPE :      TRANSFER"));
                doc.Add(new Paragraph("AMOUNT :       Rs." + transaction.Amount + ".00"));
                doc.Add(new Paragraph("\nFROM A/C :       " + transaction.Acc_No));
                doc.Add(new Paragraph("\nTO A/C :       " + transaction.TransferTo));
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
    }
}
