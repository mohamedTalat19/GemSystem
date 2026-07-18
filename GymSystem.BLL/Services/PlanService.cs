using AutoMapper;
using GemSystem.DAL.Models;
using GymSystem.BLL.Contracts;
using GymSystem.BLL.Results;
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
       private readonly IUnitOfWork _unitOfWork;
       private readonly IMapper _mapper; 

        public PlanService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork=unitOfWork;
            _mapper=mapper;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var _planRepo = _unitOfWork.GetRepository<Plan>();

            var plans = await _planRepo.GetAllAsync(ct: ct);

            return _mapper.Map<IEnumerable<PlanViewModel>>(plans);


        }

        public async Task<PlanViewModel?> GetPlanByIdAsync(int PlanId, CancellationToken ct)
        {
            var _planRepo = _unitOfWork.GetRepository<Plan>();

            var plan = await _planRepo.GetByIdAsync(PlanId, ct);
            if (plan is null)
                return null;
                return _mapper.Map<PlanViewModel>(plan);

        }

        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int PlanId, CancellationToken ct = default)
        {
            var _planRepo = _unitOfWork.GetRepository<Plan>();

            var plan =await _planRepo.GetByIdAsync(PlanId , ct);
            if (plan is null || plan.IsActive) return null;
            if (await HasActiveMembershipAsync(PlanId, ct))
                return null;
           
            return _mapper.Map<UpdatePlanViewModel>(plan);

        }

        public async Task<Result> ToggleActivationAsync(int planId, CancellationToken ct)
        {
            var _planRepo = _unitOfWork.GetRepository<Plan>();

            var plan = await _planRepo.GetByIdAsync(planId, ct);
            if(plan is null) return Result.NotFound("Plan not found");

            if (plan.IsActive && await HasActiveMembershipAsync(planId, ct))
                return Result.NotFound("Cannot deactivate a plan that has active memberships");

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;


             _planRepo.Update(plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result>0 ? Result.Ok() : Result.Fail("Failed to Toggle Plan Status");
        }

        public async Task<Result> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct)
        {
            var _planRepo = _unitOfWork.GetRepository<Plan>();

            var plan = await _planRepo.GetByIdAsync (id, ct);
            if (plan is null) return Result.NotFound("Plan not found");
            if (await HasActiveMembershipAsync(id, ct))
                return Result.NotFound("Cannot edit a plan that has active memberships");

            plan.DurationDays = model.DurationDays;
            plan.Description = model.Description;
            plan.Price = model.Price;
            plan.UpdatedAt = DateTime.Now;


            _mapper.Map(model, plan);
            _planRepo.Update(plan);
            await _unitOfWork.SaveChangesAsync(ct);
            return Result.Ok();
        }






        #region Helper Methods

        private async Task<bool> HasActiveMembershipAsync(int PlanId, CancellationToken ct)
        {
            var _membershipRepo = _unitOfWork.GetRepository<Membership>();
            return await _membershipRepo.AnyAsync(m => m.PlanId == PlanId && m.EndDate > DateTime.Now, ct);
        }
        #endregion
    }
}