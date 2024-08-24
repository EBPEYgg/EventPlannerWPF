using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventPlannerWPF.Model.Data;
using EventPlannerWPF.ViewModel.Services;
using Microsoft.EntityFrameworkCore;

namespace EventPlannerWPF.ViewModel
{
    public partial class LoginVM : ObservableObject
    {
        [ObservableProperty]
        private string _loginText;

        [ObservableProperty]
        private string _passwordText;

        [ObservableProperty]
        private string _errorMessage;

        private readonly EventPlannerContext db;

        public LoginVM()
        {
            db = new EventPlannerContext();
            db.Database.OpenConnectionAsync().Wait();
        }

        [RelayCommand]
        private void Login()
        {
            var currentUser = db.User.FirstOrDefault(u => u.Login == LoginText && u.Password == PasswordText);

            if (currentUser != null)
            {
                NavigationService.NavigateTo<MainVM>();
                db.Database.CloseConnectionAsync().Wait();
                return;
            }

            ErrorMessage = "error";
        }

        [RelayCommand]
        private void Register()
        {
            NavigationService.NavigateTo<RegisterVM>();
        }

        [RelayCommand]
        private void Quit()
        {
            db.Database.CloseConnectionAsync().Wait();
            Environment.Exit(0);
        }
    }
}