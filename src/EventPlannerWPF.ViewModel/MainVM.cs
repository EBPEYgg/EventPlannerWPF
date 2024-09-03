using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventPlannerWPF.Model.Classes;
using EventPlannerWPF.ViewModel.Services;
using System.Collections.ObjectModel;

namespace EventPlannerWPF.ViewModel
{
    public partial class MainVM : ObservableObject
    {
        #region Observable Property
        [ObservableProperty]
        private DateTime _currentDate = DateTime.Now;

        [ObservableProperty]
        private DateTime _todayDate = DateTime.Now;

        [ObservableProperty]
        private DayVM _selectedDay;

        [ObservableProperty]
        private string _aboutUser;

        [ObservableProperty]
        private int _rotateAngle;

        [ObservableProperty]
        private string _previousMonth;

        [ObservableProperty]
        private string _currentMonth;

        [ObservableProperty]
        private string _nextMonth;
        #endregion

        public User? CurrentUser => UserSession.Instance.CurrentUser;

        public ObservableCollection<DayVM> Days { get; private set; }

        public MainVM()
        {
            AboutUser = CurrentUser.Login;
            PreviousMonth = CurrentDate.AddMonths(-1).ToString("MMMM");
            CurrentMonth = CurrentDate.ToString("MMMM");
            NextMonth = CurrentDate.AddMonths(1).ToString("MMMM");
            LoadCalendar();
        }

        private void LoadCalendar()
        {
            Days = new ObservableCollection<DayVM>();
            DateTime firstDayOfMonth = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month);

            // Начало недели (понедельник) перед первым днем месяца
            int dayOfWeek = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;
            DateTime startDate = firstDayOfMonth.AddDays(-dayOfWeek);

            for (int i = 0; i < 35; i++)
            {
                DateTime date = startDate.AddDays(i);
                bool isCurrentMonth = date.Month == CurrentDate.Month;

                Days.Add(new DayVM
                {
                    DisplayText = date.Day.ToString(),
                    Opacity = isCurrentMonth ? 1.0 : 0.5,
                    Bold = isCurrentMonth ? "Demibold" : "Normal"
                });
                OnPropertyChanged(nameof(Days));
            }
        }

        [RelayCommand]
        private void ShowSelectedMonth(string monthOffsetString)
        {
            var monthOffset = int.Parse(monthOffsetString);
            CurrentDate = CurrentDate.AddMonths(monthOffset);
            PreviousMonth = CurrentDate.AddMonths(-1).ToString("MMMM");
            CurrentMonth = CurrentDate.ToString("MMMM");
            NextMonth = CurrentDate.AddMonths(1).ToString("MMMM");
            LoadCalendar();
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