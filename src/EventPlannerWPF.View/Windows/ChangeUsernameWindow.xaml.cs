using System.Windows;

namespace EventPlannerWPF.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChangeUsernameWindow.xaml.
    /// </summary>
    public partial class ChangeUsernameWindow : Window
    {
        public ChangeUsernameWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}