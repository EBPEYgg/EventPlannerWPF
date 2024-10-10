using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public static readonly DependencyProperty IconShowPasswordSourceProperty =
            DependencyProperty.Register("IconShowPasswordSource", typeof(string), typeof(IconTextBoxControl));

        public static readonly DependencyProperty IsCopyEnabledProperty =
            DependencyProperty.Register("IsCopyEnabled", typeof(bool), typeof(IconTextBoxControl), new PropertyMetadata(true));

        public IconTextBoxControl()
        {
            InitializeComponent();
            this.AddHandler(TextBox.PreviewKeyDownEvent, new KeyEventHandler(OnPreviewKeyDown), true);
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (!IsCopyEnabled)
                {
                    e.Handled = true;
                }
            }
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

        public string IconShowPasswordSource
        {
            get { return (string)GetValue(IconShowPasswordSourceProperty); }
            set { SetValue(IconShowPasswordSourceProperty, value); }
        }

        public bool IsCopyEnabled
        {
            get { return (bool)GetValue(IsCopyEnabledProperty); }
            set { SetValue(IsCopyEnabledProperty, value); }
        }
    }
}