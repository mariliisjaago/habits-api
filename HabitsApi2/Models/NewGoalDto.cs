namespace HabitsApi2.Models
{
    public class NewGoalDto
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int ParentGoalId { get; set; }
        public bool IsRoot { get; set; }
    }
}
