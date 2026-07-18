using GemSystem.DAL.AppDbContexts;
using GymSystem.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace GemSystem.Extentions
{
    public static class ProgramExtention
    {
       public static async Task IntializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                await dbContext.Database.MigrateAsync();
                logger.LogInformation("Migrations Applied To The database");
            }

            var folderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "files");
            await DataSeeder.SeedAsync(dbContext, logger, folderPath);
        }
    }
}
