using HabitsApi2.Models;
using HabitsApi2.Models.Automagic;
using Microsoft.EntityFrameworkCore;

namespace HabitsApi2.Context
{
    public class HabitContextAutomagic(DbContextOptions<HabitContextAutomagic> options) : DbContext(options)
    {
        public DbSet<GoalAutomagic> Goals { get; set; }
    }
}
