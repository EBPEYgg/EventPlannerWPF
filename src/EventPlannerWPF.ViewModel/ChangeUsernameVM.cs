using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventPlannerWPF.Model.Classes;
using EventPlannerWPF.Model.Data;
using Microsoft.EntityFrameworkCore;

namespace EventPlannerWPF.ViewModel
{
    public partial class ChangeUsernameVM : ObservableObject
    {
        [ObservableProperty]
        private string _currentName;

        [ObservableProperty]
        private string _newName;

        [ObservableProperty]
        private string _currentPassword;

        [ObservableProperty]
        private string? _newPassword = null;

        public User? CurrentUser => UserSession.Instance.CurrentUser;

        public ChangeUsernameVM()
        {
            CurrentName = CurrentUser.Name;
        }

        [RelayCommand]
        private async Task SaveUser()
        {
            if (string.IsNullOrEmpty(NewName) || (CurrentName == NewName) 
                || NewName.Length > 20 || CurrentUser == null)
            {
                return;
            }

            using (EventPlannerContext db = new EventPlannerContext())
            {
                CurrentUser.Name = NewName;

                db.User.Where(u => u.Id == CurrentUser.Id)
                    .ExecuteUpdate(s => s.SetProperty(u => u.Name, u => CurrentUser.Name));
                await db.SaveChangesAsync();
                UserSession.Instance.SetUser(CurrentUser);

                CurrentName = NewName;
                NewName = string.Empty;
            }
        }
    }
}