﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventPlannerWPF.Model.Classes;
using EventPlannerWPF.Model.Data;
using EventPlannerWPF.ViewModel.Services;
using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Выбранный пользователем день месяца.
        /// </summary>
        [ObservableProperty]
        private CalendarDay _selectedDay;

        /// <summary>
        /// Выбранный пользователем год. По умолчанию текущий год.
        /// </summary>
        [ObservableProperty]
        private int _selectedYear = DateTime.Now.Year;

        /// <summary>
        /// Логин пользователя.
        /// </summary>
        [ObservableProperty]
        private string _aboutUser;

        [ObservableProperty]
        private string _previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");

        [ObservableProperty]
        private string _currentMonth = DateTime.Now.ToString("MMMM");

        [ObservableProperty]
        private string _nextMonth = DateTime.Now.AddMonths(1).ToString("MMMM");

        /// <summary>
        /// Список с заметками пользователя.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Note> _userNotes = new();

        [ObservableProperty]
        private string _noteDescription;

        [ObservableProperty]
        private DateTime _noteEndDate;

        [ObservableProperty]
        private DateTime _noteStartDate;
        #endregion

        #region Property
        public static User CurrentUser => UserSession.Instance.CurrentUser;

        public ObservableCollection<CalendarDay> Days { get; private set; } = new();

        public ObservableCollection<int> Years { get; private set; }
        #endregion

        /// <summary>
        /// Контекст данных для БД.
        /// </summary>
        private readonly EventPlannerContext db;

        public MainVM()
        {
            AboutUser = CurrentUser.Login;
            db = new EventPlannerContext();
            LoadCalendar();
            LoadYears();
            // выбор сегодняшнего дня
            SelectDayCommand.Execute(Days.FirstOrDefault(day => day.Date == DateTime.Now.Date));
        }

        #region Methods
        /// <summary>
        /// Метод, который создает сетку календаря на выбранный месяц. 
        /// </summary>
        private void LoadCalendar()
        {
            Days = new ObservableCollection<CalendarDay>();
            DateTime firstDayOfMonth = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month);

            // Начало недели (понедельник) перед первым днем месяца
            int dayOfWeek = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;
            DateTime startDate = firstDayOfMonth.AddDays(-dayOfWeek);

            for (int i = 0; i < 35; i++)
            {
                DateTime date = startDate.AddDays(i);
                bool isCurrentMonth = date.Month == CurrentDate.Month;
                var hasNotes = db.Note
                    .Any(note => note.User.Id == CurrentUser.Id && note.StartDate.Date == date);

                Days.Add(new CalendarDay
                {
                    DisplayText = date.Day.ToString(),
                    Opacity = isCurrentMonth ? 1.0 : 0.5,
                    Bold = isCurrentMonth ? "Demibold" : "Normal",
                    Date = date,
                    HasNotes = hasNotes
                });
            }
            OnPropertyChanged(nameof(Days));
        }

        private void LoadYears()
        {
            Years = new ObservableCollection<int>();
            int startOfCurrentDecade = (CurrentDate.Year / 10) * 10;
            Years.Add(startOfCurrentDecade - 1);

            for (int year = startOfCurrentDecade; year < startOfCurrentDecade + 10; year++)
            {
                Years.Add(year);
            }

            Years.Add(startOfCurrentDecade + 10);
        }

        /// <summary>
        /// Метод, который загружает заметки текущего пользователя 
        /// для выбранного пользователем дня.
        /// </summary>
        private async Task LoadUserNotesForSelectedDayAsync()
        {
            if (CurrentUser == null || SelectedDay == null)
            {
                return;
            }

            UserNotes.Clear();
            var notesForSelectedDay = await db.Note
                .Where(note => note.User.Id == CurrentUser.Id && note.StartDate.Date == SelectedDay.Date)
                .OrderBy(note => note.EndDate)
                .ToListAsync();

            foreach (var note in notesForSelectedDay)
            {
                UserNotes.Add(note);
            }
        }

        /// <summary>
        /// Добавление новой заметки в <see cref="UserNotes"/> 
        /// с учетом сортировки по <see cref="Note.EndDate"/>.
        /// </summary>
        /// <param name="newNote">Заметка.</param>
        private void AddNoteSorted(Note newNote)
        {
            int index = 0;
            while (index < UserNotes.Count && UserNotes[index].EndDate <= newNote.EndDate)
            {
                index++;
            }
            UserNotes.Insert(index, newNote);
        }
        #endregion

        #region Commands
        /// <summary>
        /// Команда, которая позволяет пользователю 
        /// изменять выбранный в календаре месяц.
        /// </summary>
        /// <param name="monthOffsetString">На сколько месяцев идет смещение.</param>
        [RelayCommand]
        private void ShowSelectedMonth(string monthOffsetString)
        {
            UserNotes.Clear();
            var monthOffset = int.Parse(monthOffsetString);

            // если текущий месяц - декабрь, а оффсет +1, тогда Year+1
            if (CurrentDate.Month == 12 && monthOffset == 1) 
                CurrentDate.AddYears(1);

            // если текущий месяц - январь, а оффсет -1, тогда Year-1
            if (CurrentDate.Month == 1 && monthOffset == -1) 
                CurrentDate.AddYears(-1);

            CurrentDate = CurrentDate.AddMonths(monthOffset);
            PreviousMonth = CurrentDate.AddMonths(-1).ToString("MMMM");
            CurrentMonth = CurrentDate.ToString("MMMM");
            NextMonth = CurrentDate.AddMonths(1).ToString("MMMM");
            LoadCalendar();

            // при возврате пользователем на текущий месяц выбирается сегодняшний день
            if (CurrentDate.Month == TodayDate.Month)
            {
                SelectDayCommand.Execute(Days.FirstOrDefault(day => day.Date == DateTime.Now.Date));
            }
        }

        [RelayCommand]
        private void SelectYear(int selectedYear)
        {
            CurrentDate = CurrentDate.AddYears(selectedYear - CurrentDate.Year);
            LoadCalendar();
            LoadYears();
        }

        /// <summary>
        /// Команда, которая загружает список заметок 
        /// пользователя в определенный день.
        /// </summary>
        /// <param name="day">День календаря.</param>
        [RelayCommand]
        private async Task SelectDayAsync(CalendarDay day)
        {
            if (SelectedDay != null)
            {
                SelectedDay.IsSelected = false;
            }

            SelectedDay = day;

            if (SelectedDay != null)
            {
                SelectedDay.IsSelected = true;
                await LoadUserNotesForSelectedDayAsync();
            }
        }

        /// <summary>
        /// Команда, позволяющая добавить новую заметку 
        /// для текущего пользователя в выбранный пользователем день.
        /// </summary>
        [RelayCommand]
        private async Task AddNewNoteAsync()
        {
            if (CurrentUser == null || SelectedDay == null)
            {
                return;
            }

            db.Entry(CurrentUser).State = EntityState.Unchanged;

            var newNote = new Note
            {
                StartDate = SelectedDay.Date.AddHours(NoteStartDate.Hour).AddMinutes(NoteStartDate.Minute),
                EndDate = SelectedDay.Date.AddHours(NoteEndDate.Hour).AddMinutes(NoteEndDate.Minute),
                Description = NoteDescription,
                IsCompleted = false,
                User = CurrentUser
            };

            db.Note.Add(newNote);
            await db.SaveChangesAsync();
            AddNoteSorted(newNote);
            //TODO: не работает из-за модального окна
            if (!SelectedDay.HasNotes) SelectedDay.HasNotes = true;
        }

        /// <summary>
        /// Команда, которая переключает состояние завершенности выполнения заметки.
        /// </summary>
        /// <param name="note">Заметка.</param>
        [RelayCommand]
        private async Task NoteIsCompleted(Note note)
        {
            if (note == null)
            {
                return;
            }

            db.Note.Update(note);
            await db.SaveChangesAsync();
        }

        [RelayCommand]
        private void EditNote(Note note)
        {
            //NoteStartDate = note.StartDate;
            //NoteEndDate = note.EndDate;
            //NoteDescription = note.Description;
        }

        /// <summary>
        /// Команда, которая удаляет выбранную заметку.
        /// </summary>
        /// <param name="note">Заметка.</param>
        [RelayCommand]
        private async Task DeleteNote(Note note)
        {
            if (note == null)
            {
                return;
            }

            db.Note.Remove(note);
            await db.SaveChangesAsync();
            UserNotes.Remove(note);
            //TODO: SelectedDay.HasNotes = false, если была удалена последняя заметка
        }

        /// <summary>
        /// Команда, которая меняет UC на авторизацию 
        /// и очищает данные о текущем пользователе.
        /// </summary>
        [RelayCommand]
        private static void Logout()
        {
            UserSession.Instance.ClearUser();
            NavigationService.NavigateTo<LoginVM>();
        }
        #endregion

        public partial class CalendarDay : ObservableObject
        {
            public string? DisplayText { get; set; }

            public double Opacity { get; set; }

            public string Bold { get; set; }

            public DateTime Date { get; set; }

            [ObservableProperty]
            private bool _isSelected;

            [ObservableProperty]
            private bool _hasNotes;
        }
    }
}