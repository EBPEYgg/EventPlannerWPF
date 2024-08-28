using EventPlannerWPF.View.Windows;
using System.Windows;
using System.Windows.Controls;

namespace EventPlannerWPF.View.Controls
{
    /// <summary>
    /// Логика взаимодействия для EventPlannerUserControl.xaml.
    /// </summary>
    public partial class EventPlannerUserControl : UserControl
    {
        public EventPlannerUserControl()
        {
            InitializeComponent();
        }

        //временное решение
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeUsernameWindow changeUsernameWindow = new();
            changeUsernameWindow.ShowDialog();
        }
    }
}