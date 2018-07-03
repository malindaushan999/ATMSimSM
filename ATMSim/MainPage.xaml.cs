﻿using StateManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ATMSim
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private NavigationService navigationService;

        public MainPage()
        {
            InitializeComponent();
        }

        public MainPage(NavigationService navigationService)
        {
            InitializeComponent();
            this.navigationService = navigationService;
        }

        private void RunLoginPage(object param)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (param == null)
                {
                    MainWindow.Instance.IsEnabled = false;
                    LoginWindow loginWindow = new LoginWindow(navigationService);
                    loginWindow.Show();
                }

            });
        }
        
        private void InsertCardButton_Click(object sender, RoutedEventArgs e)
        {
            ATMSimCommand showLoginPageCommand = new ATMSimCommand(CommandId.ShowLoginPage, navigationService);
            Delegates.SetShowLoginPage(RunLoginPage);
            ATMSimStateManager.AddToQueue(showLoginPageCommand);
        }
    }
}
