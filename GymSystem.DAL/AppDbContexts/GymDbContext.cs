using GemSystem.DAL.Configurations;
using GemSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GemSystem.DAL.AppDbContexts
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions options) : base(options) 
        {
            
        }

        public DbSet<Plan> Plans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlanConfiguration());
        }
    }
}
