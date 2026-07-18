using GymSystem.BLL.Contracts;
using GymSystem.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GemSystem.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var result = await _sessionService.GetAllSessionsAsync(ct);
            return View(result.value);
        }

        #region Create

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropDownListsAsync(ct);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownListsAsync(ct);
                return View(model);
            }

            var result = await _sessionService.CreateSessionAsync(model, ct);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.error;
            await PopulateDropDownListsAsync(ct);
            return View(model);
        }


        public async Task PopulateDropDownListsAsync(CancellationToken ct)
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownListAsync(ct), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoriesForDropDownListAsync(ct), "Id", "Name");
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var result = await _sessionService.GetSessionByIdAsync(id, ct);
            if (result.success)
                return View(result.value);
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _sessionService.GetSessionToUpdateAsync(id, ct);
            if (result.success)
            {
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownListAsync(ct), "Id", "Name");
                return View(result.value);
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id,UpdateSessionViewModel model ,CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownListAsync(ct), "Id", "Name");
                return View(model);
            }

            var result = await _sessionService.UpateSessionAsync(id, model, ct);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
                return RedirectToAction(nameof(Index));
            }

            else
            {
                TempData["ErrorMessage"] = result.error;
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownListAsync(ct), "Id", "Name");
                return View(model);
            }
        }


        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id , CancellationToken ct)
        {
            var result = await _sessionService.GetSessionByIdAsync(id , ct);
            if (result.success)
            {
                return View(result.value);
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                    return RedirectToAction(nameof(Index));
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id , CancellationToken ct)
        {
            var result = await _sessionService.RemoveSessionAsync(id , ct);
            TempData[result.success ? "SuccessMessage" : "ErrorMessage"] = result.success ? "Session Deleted Successfully" : result.error;
            return RedirectToAction(nameof(Index));
        }



        #endregion
    }
}
