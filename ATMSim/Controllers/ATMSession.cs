using ATMSim.Models;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace ATMSim
{
    /// <summary>
    /// When a ATM session is started all the session varibles will be stored in here
    /// </summary>
    static class ATMSession
    {
        /// <summary>
        /// Gets or Sets an ATMSessionStarted property
        /// </summary>
        public static bool IsStarted { get; set; }

        /// <summary>
        /// Gets or Sets an ATMSessionPinVerfied property
        /// </summary>
        public static bool IsPinVerfied { get; set; }

        /// <summary>
        /// Gets or Sets an Account number for a session
        /// </summary>
        public static string AccountNo { get; set; }

        /// <summary>
        /// Minimum withdrawal amount
        /// </summary>
        public static double MIN_WITHDRAWAL_AMOUNT = 100;

        /// <summary>
        /// Minimum withdrawal amount
        /// </summary>
        public static double MIN_TRANSFER_AMOUNT = 500;

        /// <summary>
        /// Withdrwal amount in multiplication of this amount
        /// </summary>
        public static double MULTIPLICATION_AMOUNT = 100;

        /// <summary>
        /// DispatcherTimer object for do the timeout action.
        /// </summary>
        public static DispatcherTimer SessionTimer;
        
        /// <summary>
        /// Restarting the Session Timer
        /// </summary>
        public static void RestartSessionTimer()
        {
            SessionTimer.Stop();
            SessionTimer.Start();
            count = 0;
        }

        public static int count = 0;

        /// <summary>
        /// Sign out from the current ATM session
        /// </summary>
        public static void SignOut()
        {
            count = 0;
            SessionTimer.Stop();
            var result = WpfMessageBox.Show("Information", "Please take out your card", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Warning);
            if (result.Equals(MessageBoxResult.OK))
            {
                EndSesion();
            }
        }

        /// <summary>
        /// Timeout operation
        /// </summary>
        public static void TimeOutOperation()
        {
            count = 0;
            SessionTimer.Stop();
            
            string message = "Your session ended due to exceeding the timeout. Please take out your card.";
            var result = WpfMessageBox.Show("Warning", message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Warning, WpfMessageBox.MessageBoxType.Warning);
            if (result.Equals(MessageBoxResult.OK))
            {
                EndSesion();
            }
        }

        /// <summary>
        /// End the session after timeout
        /// </summary>
        private static void EndSesion()
        {
            IsStarted = false;
            SessionTimer = null;
            AccountNo = "";

            var wins = Application.Current.Windows;
            for (int i = 0; i < wins.Count; i++)
            {
                if (wins[i].Title.Equals("WpfMessageBox"))
                {
                    wins[i].Close();
                }
            }

            MainWindow mainWindow = MainWindow.Instance;
            mainWindow.DateTimeTimer.Stop();
            mainWindow.Title = "ATMSim";
            NavigationService navigationService = mainWindow.MainNavFrame.NavigationService;
            MainPage mainPage = new MainPage(navigationService);
            navigationService.Navigate(mainPage);
        }

        /// <summary>
        /// Cancels the current opened page and navigate to previous page.
        /// </summary>
        /// <param name="navigationService">Set NavigationService object of the current Frame.</param>
        public static void CancelCurrentOperation(NavigationService navigationService)
        {
            RestartSessionTimer();
            MainWindow mainWindow = MainWindow.Instance;
            mainWindow.Title = "ATMSim - Main";
            navigationService.GoBack();
        }
        
    }
}
