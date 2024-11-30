using HabitsApi2.Context;
using HabitsApi2.Models;
using HabitsApi2.Models.Automagic;
using Microsoft.EntityFrameworkCore;

namespace HabitsApi2.DataAccess
{
    public class GoalRepository : IGoalRepository
    {
        readonly HabitContext _dbContext;

        public GoalRepository(HabitContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Goal>> GetAllAsync()
        {
            var goals = await _dbContext.Goals.ToListAsync<Goal>();
            var roots = goals.Where(g => g.IsRoot);
            var result = new List<Goal>();

            foreach(var root in roots)
            {
                root.ProcessPostOrderNotViewModel(result, goals);
            }

            return result;
        }

        public async Task<Goal> GetLastChild(int id)
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

        private async Task<Goal> GetRecursively(int? id)
        {
            var goal = await _dbContext.Goals.FindAsync(id);
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

        public int AddNewGoal(Goal goal)
        {
            var addedGoal = _dbContext.Goals.Add(goal);
            _dbContext.SaveChanges();
            return addedGoal.Entity.Id;
        }

        public void UpdateGoal(Goal goal)
        {
            _dbContext.Goals.Update(goal);
        }

        public async Task<Goal> GetById(int id)
        {
            return await GetRecursively(id);
        }
        public async Task Delete(int id)
        {
            await _dbContext.Goals.Where(g => g.Id == id).ExecuteDeleteAsync();
        }

        public Goal GetPreviousSibling(int id)
        {
            return _dbContext.Goals.FirstOrDefault(g => g.NextSiblingId == id);
        }

        public Goal GetParent(int id)
        {
            return _dbContext.Goals.FirstOrDefault(g => g.FirstChildId == id);
        }
    }
}
