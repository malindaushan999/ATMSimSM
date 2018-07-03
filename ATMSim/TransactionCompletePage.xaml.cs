using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace ATMSim
{
    /// <summary>
    /// Interaction logic for TransactionCompletePage.xaml
    /// </summary>
    public partial class TransactionCompletePage : Page
    {
        private NavigationService _navigationService;
        public TransactionCompletePage()
        {
            InitializeComponent();
        }

        public TransactionCompletePage(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private void BackToMainButton_Click(object sender, RoutedEventArgs e)
        {
            ATMSession.CancelCurrentOperation(_navigationService);
        }

        private void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            ATMSession.SignOut();
        }
    }
}
