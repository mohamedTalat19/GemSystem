using GemSystem.DAL.AppDbContexts;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbContext;
        private readonly DbSet<TEntity> _set;
        public GenericRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
            _set = _dbContext.Set<TEntity>();
            
        }
        public async Task<int> AddAsync(TEntity entity, CancellationToken ct)
        {
            _set.Add(entity);
            return await _dbContext.SaveChangesAsync(ct);
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
        {
           return _set.AnyAsync(predicate, ct);
        }

        public async Task<int> DeleteAsync(TEntity entity, CancellationToken ct)
        {
            _set.Remove(entity);
            return await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
        {
            return await _set.FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct)
        {
            return await _set.ToListAsync(ct);
        }

        public async Task<TEntity> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _set.FindAsync(id, ct);
            return entity;
        }

        public async Task<int> UpdateAsync(TEntity entity, CancellationToken ct)
        {
            _set.Update(entity);
            return await _dbContext.SaveChangesAsync(ct);
        }
    }
}
