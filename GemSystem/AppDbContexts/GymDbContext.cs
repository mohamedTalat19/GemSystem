using GemSystem.Configurations;
using GemSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GemSystem.AppDbContexts
{
    public class GymDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=GymManagment;Trusted_Connection=true;TrustServerCertificate=true");
        }

        public DbSet<Plan> Plans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlanConfiguration());
        }
    }
}
