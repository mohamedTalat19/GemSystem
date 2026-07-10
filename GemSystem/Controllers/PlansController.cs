using GemSystem.DAL.AppDbContexts;
using GemSystem.DAL.Models;
using GymSystem.BLL.Contracts;
using GymSystem.BLL.Services;
using GymSystem.BLL.ViewModels;
using GymSystem.DAL.Repositories;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GemSystem.DAL.Controllers
{
    public class PlansController : Controller
    {

        private readonly IPlanService _planService;
        public PlansController( IPlanService planService)
        {
            _planService=planService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planService.GetAllPlansAsync(ct);

            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plan = await _planService.GetPlanByIdAsync(id, ct);

            if (plan == null)
            {
                return RedirectToAction(nameof(Index));

            }
            return View(plan);
        }

        
        
        [HttpGet]
        public async Task<IActionResult> Edit(int id , CancellationToken ct)
        {
            var plan = await _planService.GetPlanToUpdateAsync(id, ct);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan cannot be edited (not found, inactive, or has active membership";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(int id,UpdatePlanViewModel model ,CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);
            
            var result = await _planService.UpdatePlanAsync(id,model ,ct);

            if (result)
                TempData["SuccessMessage"] = "Plan updated successfully";
            else
                TempData["ErrorMessage"] = "Plan Failed To update";
                return RedirectToAction(nameof(Index));
            }

        [HttpPost]
        public async Task<IActionResult> Activate(int id , CancellationToken ct)
        {
            var result = await _planService.ToggleActivationAsync(id, ct);
            if(result)
                TempData["SuccessMessage"] = "Plan status changed";
            else
                TempData["ErrorMessage"] = "Failed To Toggle Plan Status";
            return RedirectToAction(nameof(Index));
        }



        }
}
