using HabitsApi2.Models;

namespace HabitsApi2.Services
{
    public interface IGoalsService
    {
        Task AddGoal(NewGoal newGoal);
        Task<List<GoalViewModel>> GetAll();
        Task DeleteGoal(int id);
    }
}
