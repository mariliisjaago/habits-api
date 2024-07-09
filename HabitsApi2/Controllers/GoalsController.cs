using HabitsApi2.Models;
using HabitsApi2.Services;
using Microsoft.AspNetCore.Mvc;

namespace HabitsApi2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoalsController : ControllerBase
    {
        private readonly ILogger<GoalsController> _logger;
        private readonly IGoalsService _goalsService;

        public GoalsController(ILogger<GoalsController> logger, IGoalsService goalsService)
        {
            _logger = logger;
            _goalsService = goalsService;
        }

        [HttpGet(Name = "GetGoals")]
        public async Task<IEnumerable<GoalViewModel>> Get()
        {
            return await _goalsService.GetAll();
        }

        [HttpPost(Name = "addGoal")]
        public async Task<ActionResult<bool>> AddGoal([FromBody] NewGoal newGoal)
        {
            int temp = 0;
            await _goalsService.AddGoal(newGoal);
            return true;
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteGoal(int id)
        {
            int temp = 0;
            await _goalsService.DeleteGoal(id);
            return true;
        }
    }
}
