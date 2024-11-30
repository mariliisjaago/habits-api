using HabitsApi2.Context;
using HabitsApi2.Models;
using HabitsApi2.Models.Automagic;

namespace HabitsApi2.DataAccess.Automagic
{
    public interface IGoalRepositoryAutomagic
    {
        Task<IEnumerable<GoalAutomagic>> GetAllAsync();
        Task<GoalAutomagic> GetById(int id);
        Task<GoalAutomagic> GetByIdRecursively(int id);
        GoalAutomagic AddNewGoal(GoalAutomagic goal);
        Task<GoalAutomagic> GetLastChild(int id);
        void UpdateGoal(GoalAutomagic goal);
        Task DeleteGoal(GoalAutomagic goal);
        GoalAutomagic GetPreviousSibling(GoalAutomagic goal);
        GoalAutomagic GetParent(GoalAutomagic goal);
    }
}
