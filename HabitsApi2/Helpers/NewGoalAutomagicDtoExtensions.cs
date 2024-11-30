using HabitsApi2.Models;
using HabitsApi2.Models.Automagic;

namespace HabitsApi2.Helpers
{
    public static class NewGoalAutomagicDtoExtensions
    {
        public static GoalAutomagic ToGoal(this NewGoalAutomagicDto goalDto)
        {
            return new GoalAutomagic()
            {
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                CompletedDate = null,
                Title = goalDto.Title,
                Content = goalDto.Text,
                FirstChildId = null,
                NextSiblingId = null,
                IsRoot = goalDto.IsRoot,
                IsCompleted = false,
                Type = "Work",
                UserId = goalDto.UserId
            };
        }
    }
}
