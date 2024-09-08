namespace EventPlannerWPF.Model.Classes
{
    public class Note
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public virtual User User { get; set; }
    }
}