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

        public int FirstChildId { get; set; }

        public int NextSiblingId { get; set; }
        
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

        /*public void ProcessPreOrder(List<GoalViewModel> result)
        {
            var viewModel = new GoalViewModel(Id, 0, FirstChildId, NextSiblingId, Title, Content, IsCompleted, IsFirstChild, IsRoot, null, null);
            viewModel.FirstChild = new GoalViewModel(FirstChild.Id, Id, FirstChild.FirstChildId, FirstChild.NextSiblingId, FirstChild.Title, FirstChild.Content, FirstChild.IsCompleted, FirstChild.IsFirstChild, FirstChild.IsRoot, null, null);
            viewModel.NextSibling = new GoalViewModel(NextSibling.Id, 0, NextSibling.FirstChildId, NextSibling.NextSiblingId, NextSibling.Title, NextSibling.Content, NextSibling.IsCompleted, NextSibling.IsFirstChild, NextSibling.IsRoot, null, null);

            result.Add(viewModel);
            var nextPointer = FirstChild;
            if ( nextPointer != null)
            {
                nextPointer.IsFirstChild = true;
            }
            
            while (nextPointer != null)
            {
                nextPointer.ProcessPreOrder(result);
                nextPointer = nextPointer.NextSibling;
            }
        }*/

        /*public void ProcessPostOrder(List<GoalViewModel> result)
        {
            var nextPointer = FirstChild;
            if (nextPointer != null)
            {
                nextPointer.IsFirstChild = true;
            }
            while (nextPointer != null)
            {
                nextPointer.ProcessPostOrder(result);
                nextPointer = nextPointer.NextSibling;
            }

            var viewModel = new GoalViewModel(Id, 0, FirstChildId, NextSiblingId, Title, Content, IsCompleted, IsFirstChild, IsRoot, null, null);
            if (viewModel.FirstChildId != 0)
            {
                viewModel.FirstChild = new GoalViewModel(FirstChild.Id, Id, FirstChild.FirstChildId, FirstChild.NextSiblingId, FirstChild.Title, FirstChild.Content, FirstChild.IsCompleted, FirstChild.IsFirstChild, FirstChild.IsRoot, GetViewModel(FirstChild?.FirstChild), GetViewModel(FirstChild?.NextSibling));
            }
            if (viewModel.NextSiblingId != 0)
            {
                viewModel.NextSibling = new GoalViewModel(NextSibling.Id, 0, NextSibling.FirstChildId, NextSibling.NextSiblingId, NextSibling.Title, NextSibling.Content, NextSibling.IsCompleted, NextSibling.IsFirstChild, NextSibling.IsRoot, GetViewModel(FirstChild?.FirstChild), GetViewModel(FirstChild?.NextSibling));
            }
           
            result.Add(viewModel);
        }*/

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

        /*public GoalViewModel GetViewModel(Goal goal)
        {
            if (goal == null)
            {
                return null;
            }

            return new GoalViewModel()
            {
                Id = goal.Id,
                ParentGoalId = 0,
                FirstChildId = goal.FirstChildId,
                NextSiblingId = goal.NextSiblingId,
                Title = goal.Title,
                Text = goal.Content,
                IsCompleted = goal.IsCompleted,
                CompletedDate = goal.CompletedDate,
                IsFirstChild = goal.IsFirstChild,
                FirstChild = GetViewModel(goal.FirstChild),
                NextSibling = GetViewModel(goal.NextSibling),
                IsRoot = goal.IsRoot
            };
        }*/
    }
}
