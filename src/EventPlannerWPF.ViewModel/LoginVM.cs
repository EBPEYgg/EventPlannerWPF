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
        #endregion

        /// <summary>
        /// Контекст данных для БД.
        /// </summary>
        private readonly EventPlannerContext db;

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