using System;
using System.Windows;

namespace ATMSim
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                //ATMSim.Resources.ProgressWindow progressWindow = new Resources.ProgressWindow();
                //progressWindow.Show();
                ATMSim.MainWindow.Instance.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                //throw;
            }
        }
    }
}
