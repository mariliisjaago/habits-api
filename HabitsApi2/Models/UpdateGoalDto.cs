namespace HabitsApi2.Models
{
    public class UpdateGoalDto
    {
        public int Id { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CompletedDate { get; set; }
    }
}
