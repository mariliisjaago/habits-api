using HabitsApi2.Context;
using HabitsApi2.Models;
using HabitsApi2.Models.Automagic;
using Microsoft.EntityFrameworkCore;

namespace HabitsApi2.DataAccess.Automagic
{
    public class GoalRepositoryAutomagic : IGoalRepositoryAutomagic
    {
        private readonly HabitContextAutomagic _db;
        
        public GoalRepositoryAutomagic(HabitContextAutomagic db)
        {
            _db = db;
        }

        public async Task<IEnumerable<GoalAutomagic>> GetAllAsync()
        {
            var goals = await _db.Goals.ToListAsync<GoalAutomagic>();
            return goals;
        }

        public async Task<IEnumerable<GoalAutomagic>> GetFullHierarchy(int userId)
        {
            var goals = await _db.Goals.ToListAsync<GoalAutomagic>();
            return goals;
        }

        public async Task<GoalAutomagic> GetById(int id)
        {
            return await _db.FindAsync<GoalAutomagic>(id);
        }

        public async Task<GoalAutomagic> GetByIdRecursively(int id)
        {
            return await GetRecursively(id);
        }

        public GoalAutomagic AddNewGoal(GoalAutomagic goal)
        {
            var addedGoal = _db.Goals.Add(goal);
            return addedGoal.Entity;
        }

        public async Task<GoalAutomagic> GetLastChild(int id)
        {
            var parentGoal = await GetRecursively(id);
            if (parentGoal.FirstChildId == null)
            {
                return null;
            }

            var lastChild = parentGoal.FirstChild;
            var idx = 0;
            while (lastChild?.NextSiblingId != null && idx < 1000)
            {
                lastChild = lastChild?.NextSibling;
                idx++;
            }

            return lastChild;
        }

        private async Task<GoalAutomagic> GetRecursively(int? id)
        {
            var goal = await _db.Goals.FindAsync(id);
            if (goal == null)
            {
                return null;
            }
            if (goal.FirstChildId != null)
            {
                goal.FirstChild = await GetRecursively(goal.FirstChildId);
            }
            if (goal.NextSiblingId != null)
            {
                goal.NextSibling = await GetRecursively(goal.NextSiblingId);
            }
            return goal;
        }

        public void UpdateGoal(GoalAutomagic goal)
        {
            _db.Goals.Update(goal);
        }

        public async Task DeleteGoal(GoalAutomagic goal)
        {
           
            _db.Goals.Remove(goal);
        }

        public GoalAutomagic GetPreviousSibling(GoalAutomagic goal)
        {
            return _db.Goals.FirstOrDefault(g => g.NextSibling == goal);
        }

        public GoalAutomagic GetParent(GoalAutomagic goal)
        {
            return _db.Goals.FirstOrDefault(g => g.FirstChild == goal);
        }
    }
}
