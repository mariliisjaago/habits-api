using HabitsApi2.Context;
using HabitsApi2.Models.Automagic;

namespace HabitsApi2.Services.Automagic
{
    public interface IGoalsServiceAutomagic
    {
        Task<List<GoalAutomagicViewModel>> GetAll();
        Task<GoalAutomagicViewModel> GetById(int id);
        Task AddGoal(NewGoalAutomagicDto newGoal);
        Task<int> DeleteGoal(int id);
        Task<int> UpdateGoal(UpdateGoalAutomagicDto updatedGoal);
    }
}
