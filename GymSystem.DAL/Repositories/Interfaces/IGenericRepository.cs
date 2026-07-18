using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        Task<TEntity> GetByIdAsync(int id, CancellationToken ct);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        void Add(TEntity entity);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);

        Task<int> CountAsync(Expression<Func<TEntity , bool>> ? condition = null, CancellationToken ct = default);




    }
}
