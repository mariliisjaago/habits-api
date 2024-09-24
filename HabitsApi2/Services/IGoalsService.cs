using HabitsApi2.Models;

namespace HabitsApi2.Services
{
    public interface IGoalsService
    {
        Task AddGoal(NewGoalDto newGoal);
        Task<List<GoalViewModel>> GetAll();
        Task DeleteGoal(int id);
        Task UpdateGoal(UpdateGoalDto updatedGoal);
    }
}
