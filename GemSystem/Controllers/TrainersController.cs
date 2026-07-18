using GymSystem.BLL.Contracts;
using GymSystem.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ITrainerService _trainerService;
        public TrainersController(ITrainerService trainerService)
        {
            _trainerService=trainerService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        => View(await _trainerService.GetAllTrainersAsync(ct));

        [HttpGet]

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _trainerService.CreateTrainerAsync(model, ct);
            if (result.success)
            {

                TempData["SuccessMessage"] = "Trainer Created Successfully";
                return RedirectToAction(nameof(Index));

            }
            TempData["ErrorMessage"] = result.error;
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var trainer = await _trainerService.GetTrainersDetailsAsync(id, ct);
            if (trainer is null)
            {
                TempData["ErrorMessage"] =  "Trainer not found.";
                return RedirectToAction(nameof(Index));

            }
            return View(trainer);
        }

        [HttpGet]

        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var trainer = await _trainerService.GetTrainerToUpdateAsync(id, ct);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not Found";
                return RedirectToAction(nameof(Index));

            }
            return View(trainer);


        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TrainerToUpdateViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _trainerService.UpdateTrainerDetailsAsync( id, model, ct);
            if (result.success)

                TempData["SuccessMessage"] = "Trainer updated Successfully";
            else

                TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]

        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var trainer = await _trainerService.GetTrainersDetailsAsync(id, ct);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not Found";
                return RedirectToAction(nameof(Index));

            }
            return View(trainer);


        }











        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id , CancellationToken ct)
        {
            var result = await _trainerService.RemioveTrainerAsync(id, ct);
            if (result.success)

                TempData["SuccessMessage"] = "Trainer deleted Successfully";
            else

                TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));

        }
    }

    }
