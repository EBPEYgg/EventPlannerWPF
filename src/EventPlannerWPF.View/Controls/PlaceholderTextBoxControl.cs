using System.Windows.Controls;
using System.Windows;

namespace EventPlannerWPF.View.Controls
{
    public class PlaceholderTextBoxControl : TextBox
    {
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(PlaceholderTextBoxControl),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsEmptyProperty =
            DependencyProperty.Register("IsEmpty", typeof(bool), typeof(PlaceholderTextBoxControl),
                new PropertyMetadata(false));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public bool IsEmpty
        {
            get { return (bool)GetValue(IsEmptyProperty); }
            private set { SetValue(IsEmptyProperty, value); }
        }

        static PlaceholderTextBoxControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlaceholderTextBoxControl), new FrameworkPropertyMetadata(typeof(PlaceholderTextBoxControl)));
        }

        protected override void OnInitialized(EventArgs e)
        {
            UpdateIsEmpty();
            base.OnInitialized(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            UpdateIsEmpty();
            base.OnTextChanged(e);
        }

        private void UpdateIsEmpty()
        {
            IsEmpty = string.IsNullOrEmpty(Text);
        }
    }
}