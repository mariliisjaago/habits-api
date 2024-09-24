using HabitsApi2.Models;

namespace HabitsApi2.Helpers
{
    public static class NewGoalDtoExtensions
    {
        public static Goal ToGoal(this NewGoalDto goalDto)
        {
            return new Goal()
            {
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                CompletedDate = null,
                Title = goalDto.Title,
                Content = goalDto.Text,
                FirstChildId = 0,
                NextSiblingId = 0,
                IsRoot = goalDto.IsRoot,
                IsCompleted = false,
                Type = "Work"
            };
        }
    }
}
