using System.Windows;

namespace EventPlannerWPF.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для SelectYearWindow.xaml.
    /// </summary>
    public partial class SelectYearWindow : Window
    {
        public SelectYearWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}