namespace HabitsApi2.Models.Automagic
{
    public class NewGoalAutomagicDto
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int ParentGoalId { get; set; }
        public bool IsRoot { get; set; }
        public int UserId { get; set; }
    }
}
