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

        //временное решение
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddNoteWindow addNoteWindow = new AddNoteWindow();
            addNoteWindow.ShowDialog();
        }

        //временное решение
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SelectYearWindow selectYearWindow = new SelectYearWindow();
            selectYearWindow.ShowDialog();
        }
    }
}