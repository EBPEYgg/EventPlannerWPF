using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventPlannerWPF.Model.Classes;
using EventPlannerWPF.Model.Data;
using EventPlannerWPF.ViewModel.Services;
using Microsoft.EntityFrameworkCore;

namespace EventPlannerWPF.ViewModel
{
    public partial class LoginVM : ObservableObject
    {
        #region Observable Property
        /// <summary>
        /// Возвращает и задает логин пользователя в соответствующем TextBox.
        /// </summary>
        [ObservableProperty]
        private string _loginText;

        /// <summary>
        /// Возвращает и задает пароль пользователя в соответствующем TextBox.
        /// </summary>
        [ObservableProperty]
        private string _passwordText;

        /// <summary>
        /// Возвращает и задает текст ошибки при неудачной авторизации (для теста).
        /// </summary>
        [ObservableProperty]
        private string _errorMessage;

        /// <summary>
        /// Путь до изображения с глазом.
        /// </summary>
        [ObservableProperty]
        private string _eyePath = _eyeIsOpenPath;
        #endregion

        #region Fields
        public string DisplayPassword => _isEyeClose ? PasswordText : new string('•', PasswordText?.Length ?? 0);

        /// <summary>
        /// Флаг. Нажата ли кнопка с глазом.
        /// </summary>
        private bool _isEyeClose = false;

        /// <summary>
        /// Путь до изображения с закрытым глазом.
        /// </summary>
        private static string _eyeIsClosePath = "/Resources/eye-off-gray_48x48.png";

        /// <summary>
        /// Путь до изображения с открытым глазом.
        /// </summary>
        private static string _eyeIsOpenPath = "/Resources/eye-gray_48x48.png";

        /// <summary>
        /// Контекст данных для БД.
        /// </summary>
        private readonly EventPlannerContext db;
        #endregion

        public LoginVM()
        {
            db = new EventPlannerContext();
        }

        #region Commands
        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrEmpty(LoginText) || string.IsNullOrEmpty(PasswordText))
            {
                ErrorMessage = "Please enter login and password";
                return;
            }

            var currentUser = await db.User
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == LoginText && u.Password == PasswordText);

            if (currentUser != null)
            {
                UserSession.Instance.SetUser(currentUser);
                NavigationService.NavigateTo<MainVM>();
                return;
            }

            ErrorMessage = "Invalid login or password";
        }

        [RelayCommand]
        private void Register()
        {
            ClearTextBoxes();
            NavigationService.NavigateTo<RegisterVM>();
        }

        [RelayCommand]
        private void ShowPassword()
        {
            EyePath = _isEyeClose ? _eyeIsOpenPath : _eyeIsClosePath;
            _isEyeClose = !_isEyeClose;
            OnPropertyChanged(nameof(DisplayPassword));
        }

        [RelayCommand]
        private void Quit()
        {
            Environment.Exit(0);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Метод, который очищает значения в текстовых полях.
        /// </summary>
        private void ClearTextBoxes()
        {
            LoginText = string.Empty;
            PasswordText = string.Empty;
            ErrorMessage = string.Empty;
        }
        #endregion
    }
}