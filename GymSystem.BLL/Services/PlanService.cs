using GemSystem.DAL.Models;
using GymSystem.BLL.Contracts;
using GymSystem.BLL.ViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services
{
    public class PlanService : IPlanService
    {
        private readonly IGenericRepository<Plan> _planRepo;
        private readonly IGenericRepository<Membership> _membershipRepo;

        public PlanService(IGenericRepository<Membership> membershipRepo, IGenericRepository<Plan> planRepo)
        {
            _membershipRepo=membershipRepo;
            _planRepo=planRepo;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _planRepo.GetAllAsync(ct: ct);
            return plans.Select(p => new PlanViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                DurationDays = p.DurationDays,
                IsActive = p.IsActive,
                Price = p.Price,
            });


        }

        public async Task<PlanViewModel?> GetPlanByIdAsync(int PlanId, CancellationToken ct)
        {
            var plan = await _planRepo.GetByIdAsync(PlanId, ct);
            if (plan is null)
                return null;
            else
                return new PlanViewModel()
                {
                    Name = plan.Name,
                    Description = plan.Description,
                    DurationDays = plan.DurationDays,
                    IsActive = plan.IsActive,
                    Price = plan.Price,
                };

        }

        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int PlanId, CancellationToken ct = default)
        {
            var plan =await _planRepo.GetByIdAsync(PlanId , ct);
            if (plan is null || plan.IsActive) return null;
            if (await HasActiveMembershipAsync(PlanId, ct))
                return null;
            else
                return new UpdatePlanViewModel()
                {
                    PlanName = plan.Name,
                    Price = plan.Price,
                    DurationDays= plan.DurationDays,
                    Description = plan.Description,
                };

        }

        public async Task<bool> ToggleActivationAsync(int planId, CancellationToken ct)
        {
            var plan = await _planRepo.GetByIdAsync(planId, ct);
            if(plan is null) return false;

            if (plan.IsActive && await HasActiveMembershipAsync(planId, ct))
                return false;

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;
            var result = await _planRepo.UpdateAsync(plan, ct);
            return result > 0;
        }

        public async Task<bool> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct)
        {
            var plan = await _planRepo.GetByIdAsync (id, ct);
            if (plan is null) return false;
            if (await HasActiveMembershipAsync(id, ct))
                return false;

            plan.DurationDays = model.DurationDays;
            plan.Description = model.Description;
            plan.Price = model.Price;
            plan.UpdatedAt = DateTime.Now;

            var result = await _planRepo.UpdateAsync(plan,ct);
            return result > 0;
        }






        #region Helper Methods

        private async Task<bool> HasActiveMembershipAsync(int PlanId, CancellationToken ct)
        {
            return await _membershipRepo.AnyAsync(m => m.PlanId == PlanId && m.EndDate > DateTime.Now, ct);
        }
        #endregion
    }
}