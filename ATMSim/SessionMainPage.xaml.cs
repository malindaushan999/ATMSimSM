using ATMSim.Models;
using StateManager;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ATMSim
{
    /// <summary>
    /// Interaction logic for SessionMainPage.xaml
    /// </summary>
    public partial class SessionMainPage : Page
    {
        private NavigationService _navigationService;
        private DBHelper dBHelper;
        private User currentUser;

        public SessionMainPage()
        {
            InitializeComponent();
        }

        public SessionMainPage(NavigationService navigationService)
        {
            InitializeComponent();
            this._navigationService = navigationService;
            MainWindow mainWindow = MainWindow.Instance;
            mainWindow.Title = "ATMSim - Main";
            dBHelper = new DBHelper();
            currentUser = dBHelper.GetUser(ATMSession.AccountNo);
            NameTextBlock.Text = currentUser.Name;
        }

        private void BalanceButton_Click(object sender, RoutedEventArgs e)
        {
            ATMSimCommand cmd = new ATMSimCommand(CommandId.ShowBalancePage, null);
            Delegates.SetShowBalancePage(ShowBalancePage);
            ATMSimStateManager.AddToQueue(cmd);
        }

        private void WithdrawButton_Click(object sender, RoutedEventArgs e)
        {
            ATMSimCommand cmd = new ATMSimCommand(CommandId.ShowWithdrawPage, null);
            Delegates.SetShowWithdrawPage(ShowWithdrawPage);
            ATMSimStateManager.AddToQueue(cmd);
        }

        private void ShowWithdrawPage(object param)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (param == null)
                {
                    ATMSession.RestartSessionTimer();
                    SessionWithdrawPage withdrawPage = new SessionWithdrawPage(_navigationService);
                    _navigationService.Navigate(withdrawPage);
                }
            });
        }

        private void ShowBalancePage(object param)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (param == null)
                {
                    ATMSession.RestartSessionTimer();
                    SessionBalancePage balancePage = new SessionBalancePage(_navigationService);
                    _navigationService.Navigate(balancePage);
                }
            });
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            ATMSimCommand cmd = new ATMSimCommand(CommandId.ShowTransferPage, null);
            Delegates.SetShowTransferPage(ShowTransferPage);
            ATMSimStateManager.AddToQueue(cmd);
        }

        private void ShowTransferPage(object param)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (param == null)
                {
                    ATMSession.RestartSessionTimer();
                    SessionTransferPage transferPage = new SessionTransferPage(_navigationService);
                    _navigationService.Navigate(transferPage);
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
