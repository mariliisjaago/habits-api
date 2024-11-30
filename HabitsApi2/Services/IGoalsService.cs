using HabitsApi2.Context;
using HabitsApi2.Models;

namespace HabitsApi2.Services
{
    public interface IGoalsService
    {
        Task<int> AddGoal(NewGoalDto newGoal);
        Task<List<GoalViewModel>> GetAll();
        Task<GoalViewModel> GetById(int id);
        Task DeleteGoal(int id);
        Task UpdateGoal(UpdateGoalDto updatedGoal);
    }
}
