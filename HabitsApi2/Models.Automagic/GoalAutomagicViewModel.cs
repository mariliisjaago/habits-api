using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HabitsApi2.Models.Automagic
{
    public class GoalAutomagicViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int? FirstChildId { get; set; }
        public GoalAutomagic? FirstChild { get; set; }
        public int? NextSiblingId { get; set; }
        public GoalAutomagic? NextSibling { get; set; }
        public string? Type { get; set; }
        public string? Title { get; set; }
        public string Content { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsRoot { get; set; }
        public int UserId { get; set; }

        public GoalAutomagicViewModel(
            int id,
            DateTime createdDate,
            DateTime modifiedDate,
            DateTime? completedDate,
            int? firstChildId,
            GoalAutomagic? firstChild,
            int? nextSiblingId,
            GoalAutomagic? nextSibling,
            string? type,
            string? title,
            string content,
            bool isCompleted,
            bool isRoot,
            int userId)
        {
            Id = id;
            CreatedDate = createdDate;
            ModifiedDate = modifiedDate;
            CompletedDate = completedDate;
            FirstChildId = firstChildId;
            FirstChild = firstChild;
            NextSiblingId = nextSiblingId;
            NextSibling = nextSibling;
            Type = type;
            Title = title;
            Content = content;
            IsCompleted = isCompleted;
            IsRoot = isRoot;
            UserId = userId;
        }
    }
}
