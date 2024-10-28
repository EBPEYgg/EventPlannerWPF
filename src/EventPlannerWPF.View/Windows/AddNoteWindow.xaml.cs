using System.Windows;

namespace EventPlannerWPF.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddNoteWindow.xaml.
    /// </summary>
    public partial class AddNoteWindow : Window
    {
        public AddNoteWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}