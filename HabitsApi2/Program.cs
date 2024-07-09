
using HabitsApi2.DataAccess;
using HabitsApi2.Services;
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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("WideAccess", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            builder.Services.AddDbContext<HabitContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("HabitsConnection");
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
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
