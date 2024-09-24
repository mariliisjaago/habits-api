using HabitsApi2.DataAccess;
using HabitsApi2.Helpers;
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
            return goals.Select(g => ToViewModel(g)).ToList();
        }

        /*private void ProcessPreOrder(List<Goal> goals, List<GoalViewModel> result)
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
        }*/

        private GoalViewModel ToViewModel(Goal goal)
        {
            if (goal == null)
            {
                return null;
            }
        
            
            var viewModel = new GoalViewModel(goal.Id, 0, goal.FirstChildId, goal.NextSiblingId, goal.Title, goal.Content, goal.IsCompleted, goal.CompletedDate, goal.IsFirstChild, goal.IsRoot, ToViewModel(goal.FirstChild), ToViewModel(goal.NextSibling));
            return viewModel;
        }

        public async Task AddGoal(NewGoalDto newGoal)
        {
            if (newGoal.IsRoot)
            {
                AddRootGoal(newGoal);
            }
            else
            {
                await AddNonRootGoal(newGoal);
            }
        }

        private async Task AddNonRootGoal(NewGoalDto newGoal)
        {
            var goalDbObject = newGoal.ToGoal();

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

        private void AddRootGoal(NewGoalDto newGoal)
        {
            var goalDbObject = newGoal.ToGoal();
            var addedGoalId = _goalRepository.AddNewGoal(goalDbObject);
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
                    potentialPreviousSibling.NextSiblingId = 0;
                    _goalRepository.UpdateGoal(potentialPreviousSibling);
                    await _goalRepository.Delete(id);
                }
                else
                {
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
                    potentialPreviousSibling.NextSiblingId = goal.NextSiblingId;
                    _goalRepository.UpdateGoal(potentialPreviousSibling);
                    await _goalRepository.Delete(id);
                }
                else
                {
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

        public async Task UpdateGoal(UpdateGoalDto updatedGoal)
        {
            var goal = await _goalRepository.GetById(updatedGoal.Id);
            goal.IsCompleted = updatedGoal.IsCompleted;
            goal.CompletedDate = updatedGoal.CompletedDate;
            _goalRepository.UpdateGoal(goal);
        }
    }
}
