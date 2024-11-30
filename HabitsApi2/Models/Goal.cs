using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitsApi2.Models
{
    public class Goal
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        public int? FirstChildId { get; set; }

        public int? NextSiblingId { get; set; }
        
        [NotMapped]
        public Goal FirstChild { get; set; }

        [NotMapped]
        public Goal NextSibling { get; set; }
        
        public string Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsRoot { get; set; }
        [NotMapped]
        public bool IsFirstChild { get; set; }
        public int UserId { get; set; }

        public void ProcessPostOrderNotViewModel(List<Goal> result, List<Goal> allGoals)
        {
            if (FirstChildId != 0)
            {
                var firstChild = allGoals.FirstOrDefault(g => g.Id == FirstChildId);
                FirstChild = firstChild;
            }
            if (NextSiblingId != 0)
            {
                var nextSibling = allGoals.FirstOrDefault(g => g.Id == NextSiblingId);
                NextSibling = nextSibling;
            }

            var nextPointer = FirstChild;
            if (nextPointer != null)
            {
                nextPointer.IsFirstChild = true;
            }
            while (nextPointer != null)
            {
                nextPointer.ProcessPostOrderNotViewModel(result, allGoals);
                nextPointer = nextPointer.NextSibling;
            }

            if (IsRoot)
            {
                result.Add(this);
            }
        }
    }
}
