using GymSystem.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Contracts
{
   public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default);
        Task<PlanViewModel?> GetPlanByIdAsync(int PlanId , CancellationToken ct);
        Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int PlanId , CancellationToken ct = default);
        Task<bool> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct);
        Task<bool> ToggleActivationAsync(int planId, CancellationToken ct);

    }
}
