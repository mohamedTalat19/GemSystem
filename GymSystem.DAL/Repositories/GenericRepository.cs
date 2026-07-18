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
        public void Add(TEntity entity)
        {
            _set.Add(entity);
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
        {
           return _set.AnyAsync(predicate, ct);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? condition = null, CancellationToken ct = default)
        {
            return condition == null ? await _set.AsNoTracking().CountAsync(ct) : await _set.CountAsync(condition, ct);
        }

        public void Delete(TEntity entity)
        {
            _set.Remove(entity);
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

        public void Update(TEntity entity)
        {
            _set.Update(entity);
        }
    }
}
