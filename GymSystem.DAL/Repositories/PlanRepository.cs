using GemSystem.DAL.AppDbContexts;
using GemSystem.DAL.Models;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext _dbContext;

        public PlanRepository(GymDbContext dbContext)
        {
           _dbContext = dbContext; 
           
        }

        public async Task<int> CreateAsync(Plan plan, CancellationToken ct = default)
        {
            _dbContext.Plans.Add(plan);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Plan plan, CancellationToken ct = default)
        {
            _dbContext.Plans.Remove(plan);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Plan>> GetAllAsync(bool isTracked = false, CancellationToken ct = default)
        {
            IQueryable<Plan> plans = isTracked ? _dbContext.Plans : _dbContext.Plans.AsNoTracking();
            return await plans.ToArrayAsync();
        }

        public async Task<Plan> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var plan = await _dbContext.Plans.FindAsync(id);
            return plan;
        }

        public async Task<int> UpdateAsync(Plan plan, CancellationToken ct = default)
        {
            _dbContext.Update(plan);
            return await _dbContext.SaveChangesAsync(ct);
        }
    }
}
