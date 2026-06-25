using GemSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface IPlanRepository
    {
        //GetAll
        Task<IEnumerable<Plan>> GetAllAsync(bool isTracked = false, CancellationToken ct = default);
        
        //GetById
        Task<Plan> GetByIdAsync(int id, CancellationToken ct = default);
        
        //Delete
        Task<int> DeleteAsync(Plan plan, CancellationToken ct = default);

        //Update
        Task<int> UpdateAsync(Plan plan, CancellationToken ct = default);

        //Create
        Task<int> CreateAsync(Plan plan, CancellationToken ct = default);

    }
}
