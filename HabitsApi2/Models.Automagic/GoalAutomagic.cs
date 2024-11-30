using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitsApi2.Models.Automagic
{
    public class GoalAutomagic
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        
        [ForeignKey(nameof(FirstChild))]
        public int? FirstChildId { get; set; }
        public GoalAutomagic? FirstChild { get; set; }
        
        [ForeignKey(nameof(NextSibling))]
        public int? NextSiblingId { get; set; }
        public GoalAutomagic? NextSibling { get; set; }
        public string? Type { get; set; }
        public string? Title { get; set; }
        public string Content { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsRoot { get; set; }
        public int UserId { get; set; }
    }
}
