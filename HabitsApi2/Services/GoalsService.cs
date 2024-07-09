using HabitsApi2.DataAccess;
using HabitsApi2.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitsApi2.Services
{
    public class GoalsService : IGoalsService
    {
        private IGoalRepository _goalRepository;

        public GoalsService(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;   
        }

        public async Task<List<GoalViewModel>> GetAll()
        {
            var goals = await _goalRepository.GetAllAsync();
            var result = new List<GoalViewModel>();
            ProcessPostOrder(goals.ToList(), result);
            return result;
        }

        private void ProcessPreOrder(List<Goal> goals, List<GoalViewModel> result)
        {
            var firstGoal = goals.FirstOrDefault();
            if (firstGoal != null)
            {
                firstGoal.ProcessPreOrder(result);
            }
        }

        private void ProcessPostOrder(List<Goal> goals, List<GoalViewModel> result)
        {
            var firstGoal = goals.FirstOrDefault();
            if (firstGoal != null)
            {
                firstGoal.IsRoot = true;
                firstGoal.ProcessPostOrder(result);
            }
        }

        private List<GoalViewModel> ToViewModelList(List<Goal> goals)
        {
            var result = new List<GoalViewModel>();
            foreach (var goal in goals)
            {
                result.Add(new GoalViewModel(goal.Id, 0, goal.FirstChildId, goal.NextSiblingId, goal.Title, goal.Content, goal.IsCompleted, goal.IsFirstChild, goal.IsRoot, null, null));
            }

            return result;
        }

        public async Task AddGoal(NewGoal newGoal)
        {
            var goalDbObject = new Goal()
            {
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                CompletedDate = DateTime.UtcNow,
                Title = newGoal.Title,
                Content = newGoal.Text,
                FirstChildId = 0,
                NextSiblingId = 0,
                IsRoot = newGoal.IsRoot,
                IsCompleted = false,
                Type = "Work"
            };

            var parentsLastChild = await _goalRepository.GetLastChild(newGoal.ParentGoalId);
            if (parentsLastChild != null)
            {
                var addedGoalId = _goalRepository.AddNewGoal(goalDbObject);
                parentsLastChild.NextSiblingId = addedGoalId;
                parentsLastChild.ModifiedDate = DateTime.UtcNow;
                _goalRepository.UpdateGoal(parentsLastChild);
            }
            else
            {
                var parentGoal = await _goalRepository.GetById(newGoal.ParentGoalId);
                var addedGoalId = _goalRepository.AddNewGoal(goalDbObject);
                parentGoal.FirstChildId = addedGoalId;
                parentGoal.ModifiedDate = DateTime.UtcNow;
                _goalRepository.UpdateGoal(parentGoal);
            }
        }

        public async Task DeleteGoal(int id)
        {
            var goal = await _goalRepository.GetById(id);
            if (goal == null)
            {
                return;
            }

            if (goal.FirstChild != null)
            {
                return;
            }

            if (goal.NextSibling == null)
            {
                var potentialPreviousSibling = _goalRepository.GetPreviousSibling(id);
                if (potentialPreviousSibling != null)
                {
                    // handle 1
                    potentialPreviousSibling.NextSiblingId = 0;
                    _goalRepository.UpdateGoal(potentialPreviousSibling);
                    await _goalRepository.Delete(id);
                }
                else
                {
                    // handle 4
                    var parentGoal = _goalRepository.GetParent(id);
                    if (parentGoal == null)
                    {
                        // this is a root node
                        await _goalRepository.Delete(id);
                    }
                    else
                    {
                        parentGoal.FirstChildId = 0;
                        _goalRepository.UpdateGoal(parentGoal);
                        await _goalRepository.Delete(id);
                    }
                }
            }
            
            
            if (goal.NextSibling != null)
            {
                var potentialPreviousSibling = _goalRepository.GetPreviousSibling(id);
                if (potentialPreviousSibling != null)
                {
                    // handle 2
                    potentialPreviousSibling.NextSiblingId = goal.NextSiblingId;
                    _goalRepository.UpdateGoal(potentialPreviousSibling);
                    await _goalRepository.Delete(id);
                }
                else
                {
                    // handle 3
                    var parentGoal = _goalRepository.GetParent(id);
                    if (parentGoal == null)
                    {
                        throw new Exception("Parent goal not found when trying to delete node. This should not happen");
                    }
                    else
                    {
                        parentGoal.FirstChildId = goal.NextSiblingId;
                        _goalRepository.UpdateGoal(parentGoal);
                        await _goalRepository.Delete(id);
                    }
                }
            }
        }
    }
}
