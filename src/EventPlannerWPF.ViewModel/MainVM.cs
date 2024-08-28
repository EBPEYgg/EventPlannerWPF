using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventPlannerWPF.Model.Classes;
using EventPlannerWPF.ViewModel.Services;
using System.Collections.ObjectModel;

namespace EventPlannerWPF.ViewModel
{
    public partial class MainVM : ObservableObject
    {
        private DateTime _currentDate;

        [ObservableProperty]
        private DayVM _selectedDay;

        [ObservableProperty]
        private string _aboutUser;

        [ObservableProperty]
        private int _rotateAngle;

        [ObservableProperty]
        private bool _contextMenuIsOpen = false;

        public User? CurrentUser => UserSession.Instance.CurrentUser;

        public ObservableCollection<DayVM> Days { get; private set; }

        public MainVM()
        {
            AboutUser = CurrentUser.Login;
            _currentDate = DateTime.Now;
            LoadCalendar();
        }

        private void LoadCalendar()
        {
            Days = new ObservableCollection<DayVM>();
            DateTime firstDayOfMonth = new DateTime(_currentDate.Year, _currentDate.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(_currentDate.Year, _currentDate.Month);

            // Начало недели (понедельник) перед первым днем месяца
            int dayOfWeek = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;
            DateTime startDate = firstDayOfMonth.AddDays(-dayOfWeek);

            for (int i = 0; i < 35; i++)
            {
                DateTime date = startDate.AddDays(i);
                bool isCurrentMonth = date.Month == _currentDate.Month;

                Days.Add(new DayVM
                {
                    DisplayText = date.Day.ToString(),
                    Opacity = isCurrentMonth ? 1.0 : 0.5,
                    Bold = isCurrentMonth ? "Demibold" : "Normal"
                });
            }
        }

        [RelayCommand]
        private void SelectDay(DayVM day)
        {
            if (SelectedDay != null)
                SelectedDay.IsSelected = false;

            SelectedDay = day;

            if (SelectedDay != null)
                SelectedDay.IsSelected = true;
        }

        [RelayCommand]
        private void RotateImage()
        {
            RotateAngle += 180;
            ContextMenuIsOpen = !ContextMenuIsOpen;
        }

        [RelayCommand]
        private void Logout()
        {
            UserSession.Instance.ClearUser();
            NavigationService.NavigateTo<LoginVM>();
        }

        public partial class DayVM : ObservableObject
        {
            public string DisplayText { get; set; }

            public double Opacity { get; set; }

            public string Bold { get; set; }

            [ObservableProperty]
            private bool _isSelected;

            [RelayCommand]
            private void Select()
            {
                // Вызовем метод ViewModel для выбора
            }
        }
    }
}