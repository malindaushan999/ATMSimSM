using StateManager;
using System;
using System.Windows;
using System.Windows.Threading;

namespace ATMSim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        public static MainWindow Instance { get; private set; }
        private static MainWindow mainWindow;

        public DispatcherTimer DateTimeTimer; // Time for show DateTime dynamically

        static MainWindow()
        {
            Instance = new MainWindow();
        }

        private MainWindow()
        {
            InitializeComponent();

            StartDateTimeTimer();
            MainPage mainPage = new MainPage(MainNavFrame.NavigationService);
            MainNavFrame.Navigate(mainPage);
            ATMSession.IsStarted = false;

            // declare and initialize state manager and start it as a async background thread
            ATMSimStateManager stateManager = new ATMSimStateManager();
        }


        /// <summary>
        /// Sets timestamp on the page header
        /// </summary>
        public void StartDateTimeTimer()
        {
            if (DateTimeTimer != null)
            {
                Console.WriteLine(DateTimeTimer.IsEnabled);
                if (!DateTimeTimer.IsEnabled)
                {
                    DateTimeTimer = new DispatcherTimer
                    {
                        Interval = new TimeSpan(0, 0, 1)
                    };
                    DateTimeTimer.Start();
                }
            }
            else
            {
                DateTimeTimer = new DispatcherTimer
                {
                    Interval = new TimeSpan(0, 0, 1)
                };
                DateTimeTimer.Start();
            }
        }
    }
}
