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
        Task<int> DeleteAsync(TEntity entity,  CancellationToken ct);
        Task<int> UpdateAsync(TEntity entity, CancellationToken ct);
        Task<int> AddAsync(TEntity entity, CancellationToken ct);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);






    }
}
