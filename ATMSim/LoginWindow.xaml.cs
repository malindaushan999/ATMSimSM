using ATMSim.Controllers;
using ATMSim.Models;
using StateManager;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ATMSim
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private int PinTryOuts; // store number of pin tryouts
        private const int _maxPinTryouts = 3;
        private NavigationService _navigationService;
        private string _accNo, _password;

        public LoginWindow()
        {
            InitializeComponent();
            AccountNoTextBox.Focus();
            PinTryOuts = 1;
        }

        public LoginWindow(NavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
            AccountNoTextBox.Focus();
            PinTryOuts = 1;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.IsEnabled = true;
            ATMSimCommand cancelLoginPageCommand = new ATMSimCommand(CommandId.Cancel, _navigationService);
            Delegates.SetShowLoginPage(CancelLoginPage);
            ATMSimStateManager.AddToQueue(cancelLoginPageCommand);
            this.Close();
        }

        private void CancelLoginPage(object param)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (param == null)
                {
                    MainWindow.Instance.IsEnabled = true;
                    this.Close();
                }
            });
        }

        private void AccountNoTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !ValidationController.AreAllValidNumericChars(e.Text);
            base.OnPreviewTextInput(e);
        }

        private void PinPasswordBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !ValidationController.AreAllValidNumericChars(e.Text);
            base.OnPreviewTextInput(e);
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            _accNo = AccountNoTextBox.Text;
            _password = PinPasswordBox.Password;
            ATMSimCommand signInCommand = new ATMSimCommand(CommandId.SignIn, this);
            Delegates.SetAuthentication(Authenticate);
            ATMSimStateManager.AddToQueue(signInCommand);

        }

        // try to authenticate
        private void Authenticate(object param)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (param == null)
                {
                    if (AccountNoTextBox.IsVisible)
                    {
                        if (!_accNo.Equals("") && !_password.Equals(""))
                        {
                            DBHelper dBHelper = new DBHelper();
                            if (dBHelper.CheckAccountNo(_accNo))
                            {
                                if (PinTryOuts <= _maxPinTryouts)
                                {
                                    if (dBHelper.VerifyPin(_accNo, _password))
                                    {
                                        ATMSimCommand cmd = new ATMSimCommand(CommandId.AuthenticationSuccess, null);
                                        Delegates.SetShowSessionPage(ShowSessionPage);
                                        ATMSimStateManager.AddToQueue(cmd);
                                        MainWindow.Instance.IsEnabled = true;
                                    }
                                    else
                                    {
                                        var messageBoxResult = WpfMessageBox.Show("Error", "PIN verification failed", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error, WpfMessageBox.MessageBoxType.Error);
                                        if (messageBoxResult != MessageBoxResult.OK) return;
                                        PinPasswordBox.SelectAll();
                                        PinPasswordBox.Focus();

                                        ATMSimCommand cmd;
                                        if (PinTryOuts < _maxPinTryouts)
                                        {
                                            cmd = new ATMSimCommand(CommandId.AuthenticationFailure, this);
                                            ATMSimStateManager.AddToQueue(cmd);
                                        }
                                        else if (PinTryOuts == _maxPinTryouts)
                                        {
                                            cmd = new ATMSimCommand(CommandId.PinVerificationFailure, this);
                                            ATMSimStateManager.AddToQueue(cmd);
                                            MainWindow.Instance.IsEnabled = true;
                                            this.Close();
                                        }

                                        PinTryOuts++;
                                    }
                                }
                            }
                            else
                            {
                                var messageBoxResult = WpfMessageBox.Show("Error", "Invalid account number.", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error, WpfMessageBox.MessageBoxType.Error);
                                if (messageBoxResult != MessageBoxResult.OK) return;
                                PinPasswordBox.Password = "";
                                AccountNoTextBox.SelectAll();
                                AccountNoTextBox.Focus();
                                ATMSimCommand cmd = new ATMSimCommand(CommandId.AuthenticationFailure, this);
                                ATMSimStateManager.AddToQueue(cmd);
                            }
                        }
                        else
                        {
                            var messageBoxResult = WpfMessageBox.Show("Warning", "Please fill all fields", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Warning, WpfMessageBox.MessageBoxType.Warning);
                            ATMSimCommand cmd = new ATMSimCommand(CommandId.AuthenticationFailure, this);
                            ATMSimStateManager.AddToQueue(cmd);
                            AccountNoTextBox.Focus();
                        }
                    }
                    else
                    {
                        if (!PinPasswordBox.Password.Equals(""))
                        {
                            if (PinTryOuts <= _maxPinTryouts)
                            {
                                if (PinPasswordBox.Password.Equals("1234"))
                                {
                                    this.DialogResult = true;
                                    ATMSession.AccountNo = AccountNoTextBox.Text;
                                    this.Close();
                                }
                                else
                                {
                                    var messageBoxResult = WpfMessageBox.Show("Error", "PIN verification failed", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error, WpfMessageBox.MessageBoxType.Error);
                                    if (messageBoxResult != MessageBoxResult.OK) return;
                                    if (PinTryOuts == _maxPinTryouts)
                                    {
                                        this.Close();
                                        ATMSession.SignOut();
                                    }
                                    PinTryOuts++;
                                    PinPasswordBox.Focus();
                                }
                            }
                            else
                            {

                                this.Close();
                            }

                        }
                        else
                        {
                            var messageBoxResult = WpfMessageBox.Show("Warning", "Please enter your PIN", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Warning, WpfMessageBox.MessageBoxType.Warning);
                            PinPasswordBox.Focus();
                        }
                    }
                }
            });
        }

        private void ShowSessionPage(object param)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (param == null)
                {
                    this.Close();
                    ATMSession.AccountNo = AccountNoTextBox.Text;
                    SessionPage sessionPage = new SessionPage(_navigationService);
                    ATMSession.IsStarted = true;
                    _navigationService.Navigate(sessionPage);
                }
            });
        }
    }
}
