using CommunityToolkit.Mvvm.ComponentModel;

namespace EventPlannerWPF.ViewModel.Services
{
    public static class NavigationService
    {
        public static Action<ObservableObject> Navigate { get; set; } = _ => { };

        public static void NavigateTo<TViewModel>() where TViewModel : ObservableObject, new()
        {
            var viewModel = new TViewModel();
            Navigate(viewModel);
        }
    }
}