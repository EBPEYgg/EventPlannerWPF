using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventPlannerWPF.ViewModel.Services;

namespace EventPlannerWPF.ViewModel
{
    public partial class RegisterVM : ObservableObject
    {
        public RegisterVM()
        {

        }

        [RelayCommand]
        private void Login()
        {
            NavigationService.NavigateTo<LoginVM>();
        }

        [RelayCommand]
        private void Quit()
        {
            Environment.Exit(0);
        }
    }
}