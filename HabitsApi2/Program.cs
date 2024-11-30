
using HabitsApi2.Context;
using HabitsApi2.DataAccess;
using HabitsApi2.DataAccess.Automagic;
using HabitsApi2.DataAccess.Difficult;
using HabitsApi2.Services;
using HabitsApi2.Services.Automagic;
using HabitsApi2.Services.Difficult;
using Microsoft.EntityFrameworkCore;

namespace HabitsApi2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IGoalsService, GoalsService>();
            builder.Services.AddScoped<IGoalRepository, GoalRepository>();
            builder.Services.AddScoped<IGoalsServiceAutomagic, GoalsServiceAutomagic>();
            builder.Services.AddScoped<IGoalRepositoryAutomagic, GoalRepositoryAutomagic>();
            builder.Services.AddScoped<IGoalsServiceDifficult, GoalsServiceDifficult>();
            builder.Services.AddScoped<IGoalRepositoryDifficult, GoalRepositoryDifficult>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("WideAccess", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            builder.Services.AddDbContext<HabitContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("HabitsConnection");
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).LogTo(Console.WriteLine, LogLevel.Information);
            }); 
            
            builder.Services.AddDbContext<HabitContextAutomagic>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("HabitsConnection");
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).LogTo(Console.WriteLine, LogLevel.Information);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.UseCors("WideAccess");

            

            app.Run();
        }
    }
}
