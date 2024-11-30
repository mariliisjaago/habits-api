using HabitsApi2.Context;
using HabitsApi2.Models.Automagic;
using HabitsApi2.Services.Automagic;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HabitsApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalsAutomagicController : ControllerBase
    {
        private readonly ILogger<GoalsController> _logger;
        private readonly IGoalsServiceAutomagic _goalsService;
        private readonly HabitContextAutomagic _db;

        public GoalsAutomagicController(HabitContextAutomagic db, IGoalsServiceAutomagic goalsService)
        {
            _db = db;
            _goalsService = goalsService;
        }

        [HttpGet(Name = "GetGoalsAutomagic")]
        public async Task<IEnumerable<GoalAutomagicViewModel>> Get()
        {
            return await _goalsService.GetAll();
        }

        [HttpGet("id")]
        public async Task<GoalAutomagicViewModel> GetById([FromQuery] int id)
        {
            return await _goalsService.GetById(id);
        }

        [HttpPost(Name = "AddGoalAutomagic")]
        public async Task<ActionResult<bool>> AddGoal([FromBody] NewGoalAutomagicDto newGoal)
        {
            newGoal.UserId = 2;
            await _goalsService.AddGoal(newGoal);
            _db.SaveChanges();
            return true;
        }

        [HttpPut]
        public async Task<ActionResult<int>> UpdateGoal([FromQuery] int id, [FromBody] UpdateGoalAutomagicDto updatedGoal)
        {
            await _goalsService.UpdateGoal(updatedGoal);
            _db.SaveChanges();
            return updatedGoal.Id;
        }

        [HttpDelete]
        public async Task<ActionResult<int>> DeleteGoal(int id)
        {
            await _goalsService.DeleteGoal(id);
            _db.SaveChanges();
            return id;
        }
    }
}
