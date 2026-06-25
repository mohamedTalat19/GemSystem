using GemSystem.DAL.AppDbContexts;
using GymSystem.DAL.Repositories;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GemSystem.DAL.Controllers
{
    public class PlansController : Controller
    {

        private readonly IPlanRepository _planRepo;
        public PlansController(IPlanRepository planRepository)
        {
            _planRepo = planRepository;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planRepo.GetAllAsync(false , ct);

            return View(plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var plan = await _planRepo.GetByIdAsync(id);

            if (plan == null)
            {
                return RedirectToAction(nameof(Index));

            }
            return View(plan);
        }
    }
}
