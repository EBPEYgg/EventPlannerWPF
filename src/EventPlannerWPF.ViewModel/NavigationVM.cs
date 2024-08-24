using CommunityToolkit.Mvvm.ComponentModel;

namespace EventPlannerWPF.ViewModel
{
    public partial class NavigationVM : ObservableObject
    {
        [ObservableProperty]
        private object _selectedViewModel;

        public NavigationVM()
        {
            SelectedViewModel = new LoginVM();
        }
    }
}