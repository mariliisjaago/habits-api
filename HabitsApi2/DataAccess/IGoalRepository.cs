using HabitsApi2.Context;
using HabitsApi2.Models;

namespace HabitsApi2.DataAccess
{
    public interface IGoalRepository
    {
        Task<IEnumerable<Goal>> GetAllAsync();
        Task<Goal> GetLastChild(int id);
        int AddNewGoal(Goal goal);
        void UpdateGoal(Goal goal);
        Task<Goal> GetById(int id);
        Task Delete(int id);
        Goal GetPreviousSibling(int id);
        Goal GetParent(int id);
    }
}
