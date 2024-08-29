using System.Windows;
using System.Windows.Controls;

namespace EventPlannerWPF.View.Controls
{
    /// <summary>
    /// Логика взаимодействия для IconTextBoxControl.xaml.
    /// </summary>
    public partial class IconTextBoxControl : UserControl
    {
        public static readonly DependencyProperty IconSourceProperty =
        DependencyProperty.Register("IconSource", typeof(string), typeof(IconTextBoxControl));

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(IconTextBoxControl));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(IconTextBoxControl));

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(int), typeof(IconTextBoxControl));

        public IconTextBoxControl()
        {
            InitializeComponent();
        }

        public string IconSource
        {
            get { return (string)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }
    }
}