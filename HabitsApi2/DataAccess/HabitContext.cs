using HabitsApi2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HabitsApi2.DataAccess
{
    public class HabitContext(DbContextOptions<HabitContext> options) : DbContext(options)
    {
        public DbSet<Goal> Goals { get; set; }
    }
}
