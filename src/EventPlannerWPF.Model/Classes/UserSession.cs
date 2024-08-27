namespace EventPlannerWPF.Model.Classes
{
    public class UserSession
    {
        private static UserSession _instance;

        public static UserSession Instance => _instance ??= new UserSession();

        public User? CurrentUser { get; private set; }

        private UserSession()
        {
            
        }

        public void SetUser(User user)
        {
            CurrentUser = user;
        }

        public void ClearUser()
        {
            CurrentUser = null;
        }
    }
}