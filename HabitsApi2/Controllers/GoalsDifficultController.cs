using HabitsApi2.Context;
using HabitsApi2.Models.Automagic;
using HabitsApi2.Services.Automagic;
using HabitsApi2.Services.Difficult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HabitsApi2.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class GoalsDifficultController : ControllerBase
    {
        private readonly IGoalsServiceDifficult _goalsService;
        
        public GoalsDifficultController(HabitContextAutomagic db, IGoalsServiceDifficult goalsService)
        {
            _goalsService = goalsService;
        }

        [HttpGet(Name = "GetGoalsDiff")]
        public async Task<IEnumerable<GoalAutomagicViewModel>> Get()
        {
            return await _goalsService.GetAll();
        }

        [HttpGet("id")]
        public async Task<GoalAutomagicViewModel> GetById([FromQuery] int id)
        {
            return await _goalsService.GetById(id);
        }
    }
}
