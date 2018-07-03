using StateManager;
using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace ATMSim
{
    /// <summary>
    /// Interaction logic for SessionPage.xaml
    /// </summary>
    public partial class SessionPage : Page
    {
        private NavigationService navigationService; // To store NavigationService of the MainNav
        private const int _timeoutSeconds = 15; // Time for timeout action in seconds
        private const string _accountNoLabel = "Acc: ";
        MainWindow mainWindow = MainWindow.Instance;

        public SessionPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Session page constructor
        /// </summary>
        /// <param name="navigationService">NavigarionService object</param>
        public SessionPage(NavigationService navigationService)
        {
            InitializeComponent();
            InitializeSession();
            this.navigationService = navigationService;
        }

        /// <summary>
        /// Initialized the session timeouts, Controls
        /// </summary>
        private void InitializeSession()
        {
            ATMSession.count = 0;
            mainWindow.StartDateTimeTimer();
            StartSessionTimer();
            SetTimeStamp();
            DisplayAccountNumber();
            DisplaySessionMainPage();
        }

        private void SetTimeStamp()
        {
            TimeStampTextBlock.Text = String.Concat(DateTime.Now.ToLongDateString(), " ", DateTime.Now.ToLongTimeString());
            mainWindow.DateTimeTimer.Tick += new EventHandler(DateTimeTimer_Tick);
        }

        private void DateTimeTimer_Tick(object sender, EventArgs e)
        {
            ATMSession.count++;
            TimeStampTextBlock.Text = String.Concat(DateTime.Now.ToLongDateString(), " ", DateTime.Now.ToLongTimeString());
            SessionTimerTextBlock.Text = String.Concat("Session Timer: ", ATMSession.count);
        }

        /// <summary>
        /// Show account number top right corner
        /// </summary>
        private void DisplayAccountNumber()
        {
            this.AccountNumberTextBlock.Text = String.Concat(_accountNoLabel, ATMSession.AccountNo);
        }

        private void DisplaySessionMainPage()
        {
            NavigationService sessionFrameNavigationService = SessionFrame.NavigationService;
            SessionMainPage sessionMainPage = new SessionMainPage(sessionFrameNavigationService);
            sessionFrameNavigationService.Navigate(sessionMainPage);
        }

        /// <summary>
        /// Initialize session timeout timer
        /// </summary>
        public void StartSessionTimer()
        {
            if (ATMSession.SessionTimer == null)  // if only session timer is not created then create new DispatcherTimer()
            {
                ATMSession.SessionTimer = new DispatcherTimer();
                ATMSession.SessionTimer.Tick += new EventHandler(SessionTimer_Tick);
                ATMSession.SessionTimer.Interval = new TimeSpan(0, 0, _timeoutSeconds);
                ATMSession.SessionTimer.Start();
            }
            
        }

        /// <summary>
        /// Sign out the user from the session
        /// </summary>
        private void SessionTimer_Tick(object sender, EventArgs e)
        {
            ATMSimCommand cmd = new ATMSimCommand(CommandId.Timeout, null);
            Delegates.SetTimeOutOp(TimeOutOp);
            ATMSimStateManager.AddToQueue(cmd);
        }

        private void TimeOutOp(object param)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (param == null)
                {
                    ATMSession.TimeOutOperation();
                }
            });
        }

        public void SetTitle(string title)
        {
            TitleTextBlock.Text = String.Concat("ATMSim - ", title);
        }
    }
}
