using GemSystem.DAL.AppDbContexts;
using GemSystem.DAL.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymSystem.DAL
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(GymDbContext context, ILogger logger, string folderPath, CancellationToken ct = default)
        {
            try
            {
                if (context.Plans.Any()) return;
                var plans = LoadDataFromJsonFile<Plan>(folderPath, "plans.json");
                if (plans != null)
                {
                    context.Plans.AddRange(plans);
                    logger.LogInformation("Plans seeded localy");
                }
                if (context.ChangeTracker.HasChanges())
                {
                    await context.SaveChangesAsync(ct);
                    logger.LogInformation("Plans seeded Successfully");
                }
                else
                    logger.LogInformation("Plans already seeded");
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}"); 
            }
        }

        private static List<T> LoadDataFromJsonFile<T>(string folderPath, string FileName)
        {
            var filePath = Path.Combine(folderPath, FileName);
            if (!File.Exists(filePath)) throw new FileNotFoundException("Seed File Wasn't Found");

            var data = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<List<T>>(data, options) ?? []; 
        }
        
    }
}
