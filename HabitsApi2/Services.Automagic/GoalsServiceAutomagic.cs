using HabitsApi2.Context;
using HabitsApi2.DataAccess.Automagic;
using HabitsApi2.Helpers;
using HabitsApi2.Models.Automagic;

namespace HabitsApi2.Services.Automagic
{
    public class GoalsServiceAutomagic : IGoalsServiceAutomagic
    {
        private IGoalRepositoryAutomagic _goalRepository;

        public GoalsServiceAutomagic(IGoalRepositoryAutomagic goalRepository)
        {
            _goalRepository = goalRepository;
        }

        public async Task<List<GoalAutomagicViewModel>> GetAll()
        {
            var goals = await _goalRepository.GetAllAsync();
            return goals.Where(g => g.IsRoot).Select(g => ToViewModel(g)).ToList();
        }

        public async Task AddGoal(NewGoalAutomagicDto newGoal)
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

        private async Task AddNonRootGoal(NewGoalAutomagicDto newGoal)
        {
            GoalAutomagic goalDbObject = newGoal.ToGoal();
            var parentsLastChild = await _goalRepository.GetLastChild(newGoal.ParentGoalId);
            if (parentsLastChild != null)
            {
                var addedGoal = _goalRepository.AddNewGoal(goalDbObject);
                parentsLastChild.NextSibling = addedGoal;
                parentsLastChild.ModifiedDate = DateTime.UtcNow;
                _goalRepository.UpdateGoal(parentsLastChild);
            }
            else
            {
                var parentGoal = await _goalRepository.GetById(newGoal.ParentGoalId);
                var addedGoal = _goalRepository.AddNewGoal(goalDbObject);
                parentGoal.FirstChild = addedGoal;
                parentGoal.ModifiedDate = DateTime.UtcNow;
                _goalRepository.UpdateGoal(parentGoal);
            }
        }

        private void AddRootGoal(NewGoalAutomagicDto newGoal)
        {
            GoalAutomagic goalAutomagic = newGoal.ToGoal();
            var addedGoal = _goalRepository.AddNewGoal(goalAutomagic);
        }

        private GoalAutomagicViewModel ToViewModel(GoalAutomagic g)
        {
            if (g == null)
            {
                return null;
            }

            return new GoalAutomagicViewModel(g.Id, g.CreatedDate, g.ModifiedDate, g.CompletedDate, g.FirstChildId, g.FirstChild, g.NextSiblingId, g.NextSibling, g.Type, g.Title, g.Content, g.IsCompleted, g.IsRoot, g.UserId);
        }

        public async Task<int> DeleteGoal(int id)
        {
            var goal = await _goalRepository.GetById(id);
            if (goal == null)
            {
                return -1;
            }

            if (goal.FirstChild != null)
            {
                return -1; // this goal is a parent to another goal, not deleting
            }

            if (goal.NextSibling == null)
            {
                var previousSibling = _goalRepository.GetPreviousSibling(goal);
                if (previousSibling != null)
                {
                    previousSibling.NextSibling = null;
                    _goalRepository.UpdateGoal(previousSibling);
                    await _goalRepository.DeleteGoal(goal);
                }
                else
                {
                    var parentGoal = _goalRepository.GetParent(goal);
                    if (parentGoal == null)
                    {
                        // this is a root node
                        await _goalRepository.DeleteGoal(goal);
                    }
                    else
                    {
                        parentGoal.FirstChild = null;
                        _goalRepository.UpdateGoal(parentGoal);
                        await _goalRepository.DeleteGoal(goal);
                    }
                }

                
            }

            if (goal.NextSibling != null)
            {
                var previousSibling = _goalRepository.GetPreviousSibling(goal);
                if (previousSibling != null)
                {
                    previousSibling.NextSibling = goal.NextSibling;
                    _goalRepository.UpdateGoal(previousSibling);
                    await _goalRepository.DeleteGoal(goal);
                }
                else
                {
                    var parentGoal = _goalRepository.GetParent(goal);
                    if (parentGoal == null)
                    {
                        throw new Exception("Parent goal not found when trying to delete node. This should not happen");
                    }
                    else
                    {
                        parentGoal.FirstChild = goal.NextSibling;
                        _goalRepository.UpdateGoal(parentGoal);
                        await _goalRepository.DeleteGoal(goal);
                    }
                }
            }

            return id;
        }

        public async Task<int> UpdateGoal(UpdateGoalAutomagicDto updatedGoal)
        {
            var goal = await _goalRepository.GetById(updatedGoal.Id);
            goal.IsCompleted = updatedGoal.IsCompleted;
            goal.CompletedDate = updatedGoal.CompletedDate;
            _goalRepository.UpdateGoal(goal);
            return goal.Id;
        }

        public async Task<GoalAutomagicViewModel> GetById(int id)
        {
            var dbGoal = await _goalRepository.GetByIdRecursively(id);
            return ToViewModel(dbGoal);
        }
    }
}
