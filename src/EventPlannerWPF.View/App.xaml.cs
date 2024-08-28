using EventPlannerWPF.View.Controls;
using EventPlannerWPF.View.Windows;
using EventPlannerWPF.ViewModel;
using EventPlannerWPF.ViewModel.Services;
using System.Windows;

namespace EventPlannerWPF.View
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = new MainWindow();

            NavigationService.Navigate = viewModel =>
            {
                if (viewModel is LoginVM)
                {
                    mainWindow.Content = new LoginUserControl { DataContext = viewModel };
                }
                else if (viewModel is MainVM)
                {
                    mainWindow.Content = new EventPlannerUserControl { DataContext = viewModel };
                }
                else if (viewModel is RegisterVM)
                {
                    mainWindow.Content = new RegisterUserControl { DataContext = viewModel };
                }
            };
            
            mainWindow.Show();
        }
    }
}