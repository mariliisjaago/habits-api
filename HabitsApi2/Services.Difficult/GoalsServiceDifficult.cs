using HabitsApi2.DataAccess.Difficult;
using HabitsApi2.Models.Automagic;

namespace HabitsApi2.Services.Difficult
{
    public class GoalsServiceDifficult : IGoalsServiceDifficult
    {
        private readonly IGoalRepositoryDifficult _goalRepository;
        
        public GoalsServiceDifficult(IGoalRepositoryDifficult goalRepository)
        {
            _goalRepository = goalRepository;
        }

        public async Task<List<GoalAutomagicViewModel>> GetAll()
        {
            var goals = await _goalRepository.GetAllAsync();
            return goals.Select(g => ToViewModel(g)).ToList();
        }

        public async Task<GoalAutomagicViewModel> GetById(int id)
        {
            var dbGoal = await _goalRepository.GetById(id);
            return ToViewModel(dbGoal);
        }

        private GoalAutomagicViewModel ToViewModel(GoalAutomagic g)
        {
            if (g == null)
            {
                return null;
            }

            return new GoalAutomagicViewModel(g.Id, g.CreatedDate, g.ModifiedDate, g.CompletedDate, g.FirstChildId, g.FirstChild, g.NextSiblingId, g.NextSibling, g.Type, g.Title, g.Content, g.IsCompleted, g.IsRoot, g.UserId);
        }
    }
}
