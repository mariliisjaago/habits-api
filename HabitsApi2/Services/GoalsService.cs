using HabitsApi2.Context;
using HabitsApi2.DataAccess;
using HabitsApi2.Helpers;
using HabitsApi2.Models;

namespace HabitsApi2.Services
{
    public class GoalsService : IGoalsService
    {
        private readonly IGoalRepository _goalRepository;
        private readonly HabitContext _db;

        public GoalsService(IGoalRepository goalRepository, HabitContext db)
        {
            _goalRepository = goalRepository;
            _db = db;
        }

        public async Task<List<GoalViewModel>> GetAll()
        {
            var goals = await _goalRepository.GetAllAsync();
            return goals.Select(g => ToViewModel(g)).ToList();
        }

        private GoalViewModel ToViewModel(Goal goal)
        {
            if (goal == null)
            {
                return null;
            }
        
            
            var viewModel = new GoalViewModel(goal.Id, 0, goal.FirstChildId, goal.NextSiblingId, goal.Title, goal.Content, goal.IsCompleted, goal.CompletedDate, goal.IsFirstChild, goal.IsRoot, ToViewModel(goal.FirstChild), ToViewModel(goal.NextSibling), goal.UserId);
            return viewModel;
        }

        public async Task<int> AddGoal(NewGoalDto newGoal)
        {
            if (newGoal.IsRoot)
            {
                return AddRootGoal(newGoal);
            }
            else
            {
                return await AddNonRootGoal(newGoal);
            }
        }

        private async Task<int> AddNonRootGoal(NewGoalDto newGoal)
        {
            var goalDbObject = newGoal.ToGoal();

            var parentsLastChild = await _goalRepository.GetLastChild(newGoal.ParentGoalId);
            if (parentsLastChild != null)
            {
                var addedGoalId = _goalRepository.AddNewGoal(goalDbObject);
                
                parentsLastChild.NextSiblingId = addedGoalId;
                parentsLastChild.ModifiedDate = DateTime.UtcNow;
                _goalRepository.UpdateGoal(parentsLastChild);
                return addedGoalId;
            }
            else
            {
                var parentGoal = await _goalRepository.GetById(newGoal.ParentGoalId);
                var addedGoalId = _goalRepository.AddNewGoal(goalDbObject);
                parentGoal.FirstChildId = addedGoalId;
                parentGoal.ModifiedDate = DateTime.UtcNow;
                _goalRepository.UpdateGoal(parentGoal);
                return addedGoalId;
            }
        }

        private int AddRootGoal(NewGoalDto newGoal)
        {
            var goalDbObject = newGoal.ToGoal();
            return _goalRepository.AddNewGoal(goalDbObject);
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
                    potentialPreviousSibling.NextSiblingId = null;
                    potentialPreviousSibling.NextSibling = null;
                    _goalRepository.UpdateGoal(potentialPreviousSibling);
                    _db.SaveChanges();
                    await _goalRepository.Delete(id);
                }
                else
                {
                    var parentGoal = _goalRepository.GetParent(id);
                    if (parentGoal == null)
                    {
                        // this is a root node
                        await _goalRepository.Delete( id);
                    }
                    else
                    {
                        parentGoal.FirstChildId = null;
                        parentGoal.FirstChild = null;
                        _goalRepository.UpdateGoal(parentGoal);
                        _db.SaveChanges();
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
                    _db.SaveChanges();
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
                        _db.SaveChanges();
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

        public async Task<GoalViewModel> GetById(int id)
        {
            var dbGoal = await _goalRepository.GetById(id);
            return ToViewModel(dbGoal);
        }
    }
}
