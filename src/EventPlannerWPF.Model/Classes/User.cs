namespace EventPlannerWPF.Model.Classes
{
    public class User
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public DateTime Date { get; set; }

        public virtual ICollection<Note> Note { get; set; }

        public User()
        {

        }

        public User(string userlogin, string password, DateTime date)
        {
            Login = userlogin;
            Password = password;
            Date = date;
        }
    }
}