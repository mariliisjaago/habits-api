using HabitsApi2.Context;
using HabitsApi2.Models.Automagic;

namespace HabitsApi2.DataAccess.Difficult
{
    public interface IGoalRepositoryDifficult
    {
        Task<IEnumerable<GoalAutomagic>> GetAllAsync();
        Task<GoalAutomagic> GetById(int id);
    }
}
