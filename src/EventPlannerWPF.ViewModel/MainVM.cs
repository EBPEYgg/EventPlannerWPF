using CommunityToolkit.Mvvm.ComponentModel;
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

        [ObservableProperty]
        private DayVM _selectedDay;

        [ObservableProperty]
        private string _aboutUser;

        [ObservableProperty]
        private string _previousMonth;

        [ObservableProperty]
        private string _currentMonth;

        [ObservableProperty]
        private string _nextMonth;

        /// <summary>
        /// Список с заметками пользователя.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Note> _userNotes = new ObservableCollection<Note>();

        [ObservableProperty]
        private string _noteDescription;

        [ObservableProperty]
        private DateTime _noteEndDate;

        [ObservableProperty]
        private DateTime _noteStartDate;
        #endregion

        #region Property
        public User? CurrentUser => UserSession.Instance.CurrentUser;

        public ObservableCollection<DayVM> Days { get; private set; }
        #endregion

        /// <summary>
        /// Контекст данных для БД.
        /// </summary>
        private readonly EventPlannerContext db;

        public MainVM()
        {
            AboutUser = CurrentUser.Login;
            PreviousMonth = CurrentDate.AddMonths(-1).ToString("MMMM");
            CurrentMonth = CurrentDate.ToString("MMMM");
            NextMonth = CurrentDate.AddMonths(1).ToString("MMMM");
            db = new EventPlannerContext();
            LoadCalendar();
            SelectDayCommand.Execute(Days.FirstOrDefault(day => day.Date == DateTime.Now.Date));
        }

        #region Methods
        /// <summary>
        /// Метод, который создает сетку календаря на выбранный месяц. 
        /// </summary>
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
                    Bold = isCurrentMonth ? "Demibold" : "Normal",
                    Date = date
                });
                OnPropertyChanged(nameof(Days));
            }
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

        /// <summary>
        /// Команда, которая позволяет пользователю выделить 
        /// день календаря и соответствующую ячейку в календаре.
        /// </summary>
        /// <param name="day">День календаря.</param>
        [RelayCommand]
        private async Task SelectDayAsync(DayVM day)
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
                StartDate = DateTime.Today.Add(new TimeSpan(0, 0, 0)),
                EndDate = NoteEndDate,
                Description = NoteDescription,
                IsCompleted = false,
                User = CurrentUser
            };

            db.Note.Add(newNote);
            await db.SaveChangesAsync();
            AddNoteSorted(newNote);
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

        public partial class DayVM : ObservableObject
        {
            public string DisplayText { get; set; }

            public double Opacity { get; set; }

            public string Bold { get; set; }

            public DateTime Date { get; set; }

            [ObservableProperty]
            private bool _isSelected;
        }
    }
}