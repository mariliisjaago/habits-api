using HabitsApi2.Context;
using HabitsApi2.Models;
using HabitsApi2.Models.Automagic;
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
        private readonly HabitContext _db;

        public GoalsController(HabitContext db, ILogger<GoalsController> logger, IGoalsService goalsService)
        {
            _logger = logger;
            _goalsService = goalsService;
            _db = db;
        }

        [HttpGet(Name = "GetGoals")]
        public async Task<IEnumerable<GoalViewModel>> Get()
        {
            return await _goalsService.GetAll();
        }

        [HttpGet("id")]
        public async Task<GoalViewModel> GetById([FromQuery] int id)
        {
            return await _goalsService.GetById(id);
        }

        [HttpPost(Name = "AddGoal")]
        public async Task<ActionResult<int>> AddGoal([FromBody] NewGoalDto newGoal)
        {
            newGoal.UserId = 1;
            var addedGoalId = await _goalsService.AddGoal(newGoal);
            _db.SaveChanges();
            return addedGoalId;
        }

        [HttpDelete]
        public async Task<ActionResult<int>> DeleteGoal(int id)
        {
            await _goalsService.DeleteGoal(id);
            _db.SaveChanges();
            return id;
        }

        [HttpPut]
        public async Task<ActionResult<int>> UpdateGoal([FromQuery] int id, [FromBody] UpdateGoalDto updatedGoal)
        {
            await _goalsService.UpdateGoal(updatedGoal);
            _db.SaveChanges();
            return id;
        }
    }
}
