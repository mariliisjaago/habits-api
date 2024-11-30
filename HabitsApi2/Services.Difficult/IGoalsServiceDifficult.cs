using HabitsApi2.Context;
using HabitsApi2.Models.Automagic;

namespace HabitsApi2.Services.Difficult
{
    public interface IGoalsServiceDifficult
    {
        Task<List<GoalAutomagicViewModel>> GetAll();

        Task<GoalAutomagicViewModel> GetById(int id);
    }
}
