using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventPlannerWPF.Model.Classes;
using EventPlannerWPF.Model.Data;
using EventPlannerWPF.ViewModel.Services;
using Microsoft.EntityFrameworkCore;

namespace EventPlannerWPF.ViewModel
{
    public partial class RegisterVM : ObservableObject
    {
        #region Observable Property
        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _userLogin;

        [ObservableProperty]
        private string _password;
        #endregion

        public RegisterVM()
        {

        }

        #region Commands
        /// <summary>
        /// Метод, который создает новый аккаунт пользователя в базе данных 
        /// и перенаправляет пользователя на главную страницу.
        /// </summary>
        [RelayCommand]
        private async Task CreateAccountAsync()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(UserLogin) || string.IsNullOrEmpty(Password))
            {
                return;
            }

            using (EventPlannerContext db = new EventPlannerContext())
            {
                db.Database.OpenConnectionAsync().Wait();
                User newUser = new User(UserLogin, Password, DateTime.Now);
                db.User.Add(newUser);
                await db.SaveChangesAsync();
                UserSession.Instance.SetUser(newUser);
                ClearTextBoxes();
                OpenEventPlannerUC();
            }
        }

        /// <summary>
        /// Метод, который меняет текущий UC на Login UC.
        /// </summary>
        [RelayCommand]
        private void OpenLoginUC()
        {
            NavigationService.NavigateTo<LoginVM>();
        }

        /// <summary>
        /// <inheritdoc cref="Environment.Exit(int)"/>
        /// </summary>
        [RelayCommand]
        private void Quit()
        {
            Environment.Exit(0);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Метод, который меняет текущий UC на главный UC.
        /// </summary>
        private void OpenEventPlannerUC()
        {
            NavigationService.NavigateTo<MainVM>();
        }

        /// <summary>
        /// Метод, который очищает значения в текстовых полях на форме.
        /// </summary>
        private void ClearTextBoxes()
        {
            Email = string.Empty;
            UserLogin = string.Empty;
            Password = string.Empty;
        }
        #endregion
    }
}