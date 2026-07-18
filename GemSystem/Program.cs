using AutoMapper;
using GemSystem.DAL.AppDbContexts;
using GemSystem.Extentions;
using GymSystem.BLL.Contracts;
using GymSystem.BLL.MappingProfiles;
using GymSystem.BLL.Services;
using GymSystem.DAL;
using GymSystem.DAL.Repositories;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GemSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //builder.Services.AddScoped<IPlanRepository, PlanRepository>();

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<IUnitOfWork , UnitOfWork>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ITrainerService,TrainerService>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<ISessionService,SessionSevice >();
            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            var app = builder.Build();

            await app.IntializeDatabaseAsync();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
