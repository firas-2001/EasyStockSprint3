using EasyStock.Models.ViewModels;
using EasyStock.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EasyStock.Controllers
{
    [TypeFilter(typeof(SessionAuthFilter), Arguments = new object[] { "Gestionnaire,Operateur" })]
    public class AffectationsController : Controller
    {
        private readonly AffectationService _affectationService;

        public AffectationsController(AffectationService affectationService)
        {
            _affectationService = affectationService;
        }

        public async Task<IActionResult> Index(string searchField = "Nom", string? searchTerm = null)
        {
            ViewBag.SearchField = searchField;
            ViewBag.SearchTerm = searchTerm;
            return View(await _affectationService.GetActiveAsync(searchField, searchTerm));
        }

        public async Task<IActionResult> History(string searchField = "Nom", string? searchTerm = null)
        {
            ViewBag.SearchField = searchField;
            ViewBag.SearchTerm = searchTerm;
            return View(await _affectationService.GetHistoryAsync(searchField, searchTerm));
        }

        public async Task<IActionResult> Details(int? id)
        {
            var affectation = await _affectationService.GetByIdAsync(id);
            return affectation == null ? NotFound() : View(affectation);
        }

        public async Task<IActionResult> Create(string searchField = "Nom", string? searchTerm = null, int? employeId = null)
        {
            var model = new AffectationCreateViewModel
            {
                SearchField = searchField,
                SearchTerm = searchTerm,
                EmployeId = employeId ?? 0
            };

            await PopulateSelectionsAsync(model.ArticleId, employeId, searchField, searchTerm);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AffectationCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateSelectionsAsync(model.ArticleId, model.EmployeId, model.SearchField, model.SearchTerm);
                return View(model);
            }

            try
            {
                await _affectationService.CreateAsync(model, GetActor(), GetUserId());
                TempData["SuccessMessage"] = "Affectation enregistrée avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateSelectionsAsync(model.ArticleId, model.EmployeId, model.SearchField, model.SearchTerm);
                return View(model);
            }
        }

        public async Task<IActionResult> Return(int? id)
        {
            var affectation = await _affectationService.GetByIdAsync(id);
            if (affectation == null)
            {
                return NotFound();
            }

            ViewBag.Affectation = affectation;
            return View(new AffectationReturnViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id, AffectationReturnViewModel model)
        {
            var affectation = await _affectationService.GetByIdAsync(id);
            if (affectation == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Affectation = affectation;
                return View(model);
            }

            try
            {
                await _affectationService.ReturnAsync(id, model, GetActor(), GetUserId());
                TempData["SuccessMessage"] = "Désaffectation enregistrée avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.Affectation = affectation;
                return View(model);
            }
        }

        private async Task PopulateSelectionsAsync(int? articleId = null, int? employeId = null, string? searchField = null, string? searchTerm = null)
        {
            ViewData["ArticleId"] = new SelectList(await _affectationService.GetAssignableArticlesAsync(), "ArticleId", "Name", articleId);
            var employes = await _affectationService.GetAssignableEmployesAsync(searchField, searchTerm);
            ViewData["EmployeId"] = new SelectList(
                employes.Select(e => new { e.EmployeId, Label = $"{e.EmployeId} - {e.Prenom} {e.Nom} - {e.Departement} - {e.Poste}" }),
                "EmployeId",
                "Label",
                employeId);
            ViewBag.SearchField = searchField ?? "Nom";
            ViewBag.SearchTerm = searchTerm;
            ViewBag.HasEmployes = employes.Any();
        }

        private string GetActor()
        {
            return HttpContext.Session.GetString("Username") ?? "system";
        }

        private int? GetUserId()
        {
            return HttpContext.Session.GetInt32("UserId");
        }
    }
}




