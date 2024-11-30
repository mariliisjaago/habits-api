using HabitsApi2.Context;
using HabitsApi2.Models.Automagic;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System;

namespace HabitsApi2.DataAccess.Difficult
{
    public class GoalRepositoryDifficult : IGoalRepositoryDifficult
    {
        private readonly HabitContextAutomagic _db;
        
        public GoalRepositoryDifficult(HabitContextAutomagic db)
        {
            _db = db;
        }

        public async Task<IEnumerable<GoalAutomagic>> GetAllAsync()
        {
            var rootIds = _db.Goals.Where(g => g.IsRoot).Select(g => g.Id).ToList();

            List<GoalAutomagic> goals = new List<GoalAutomagic>();
            foreach (var id in rootIds)
            {
                var rootHierarchy = await GetHierarchy(id);
                goals.Add(rootHierarchy);
            }

            return goals;
        }

        public async Task<GoalAutomagic> GetById(int id)
        {
            return await GetHierarchy(id);
        }

        public async Task<GoalAutomagic> GetHierarchy(int id)
        {
            var sql = @"
    WITH RECURSIVE RecursiveGoals AS (
    SELECT * 
    FROM Goals 
    WHERE Id = @RootGoalId
    UNION ALL
    SELECT g.* 
    FROM Goals g
    INNER JOIN RecursiveGoals rg 
        ON g.Id = rg.FirstChildId OR g.Id = rg.NextSiblingId
)
SELECT * FROM RecursiveGoals;"
            ;

            var parameters = new MySqlParameter("@RootGoalId", id);
            var goals = await _db.Goals.FromSqlRaw(sql, parameters).ToListAsync();
            var goal = goals.FirstOrDefault(g => g.Id == id);

            return goal;
        }
    }
}
