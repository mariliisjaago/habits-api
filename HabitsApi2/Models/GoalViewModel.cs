using System.Runtime.CompilerServices;

namespace HabitsApi2.Models
{
    public class GoalViewModel
    {
        public int Id { get; set; }
        public int ParentGoalId { get; set; }
        public int? FirstChildId { get; set; }
        public int? NextSiblingId { get; set; }
        public GoalViewModel? FirstChild { get; set; }
        public GoalViewModel? NextSibling { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
        public bool IsFirstChild { get; set; }
        public bool IsRoot { get; set; }
        public int UserId { get; set; }

        public GoalViewModel(int id, int parentGoalId, int? firstChildId, int? nextSiblingId, string title, string text, bool isCompleted, DateTime? completedDate, bool isFirstChild, bool isRoot, GoalViewModel firstChild, GoalViewModel nextSibling, int userId)
        {
            Id = id;
            ParentGoalId = parentGoalId;
            FirstChildId = firstChildId;
            NextSiblingId = nextSiblingId;
            Title = title;
            Text = text;
            IsCompleted = isCompleted;
            CompletedDate = completedDate;
            IsFirstChild = isFirstChild;
            IsRoot = isRoot;
            FirstChild = firstChild;
            NextSibling = nextSibling;
            UserId = userId;
        }

        public GoalViewModel()
        {
            
        }
    }
}
