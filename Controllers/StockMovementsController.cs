using EasyStock.Models;
using EasyStock.Models.ViewModels;
using EasyStock.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EasyStock.Controllers
{
    [TypeFilter(typeof(SessionAuthFilter), Arguments = new object[] { "Gestionnaire,Operateur" })]
    public class StockMovementsController : Controller
    {
        private readonly StockMovementService _stockMovementService;

        public StockMovementsController(StockMovementService stockMovementService)
        {
            _stockMovementService = stockMovementService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _stockMovementService.GetRecentAsync());
        }

        public async Task<IActionResult> Entry()
        {
            await PopulateArticlesAsync();
            ConfigureView("Entrée de stock", "Enregistrez une livraison, un achat ou un réapprovisionnement.", "Enregistrer l'entrée", true);
            return View(new StockMovementFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Entry(StockMovementFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateArticlesAsync(model.ArticleId);
                ConfigureView("Entrée de stock", "Enregistrez une livraison, un achat ou un réapprovisionnement.", "Enregistrer l'entrée", true);
                return View(model);
            }

            try
            {
                await _stockMovementService.RegisterEntryAsync(model, GetActor(), GetUserId());
                TempData["SuccessMessage"] = "Entrée de stock enregistrée avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateArticlesAsync(model.ArticleId);
                ConfigureView("Entrée de stock", "Enregistrez une livraison, un achat ou un réapprovisionnement.", "Enregistrer l'entrée", true);
                return View(model);
            }
        }

        public async Task<IActionResult> Output()
        {
            await PopulateArticlesAsync();
            ConfigureView("Sortie de stock", "Enregistrez une consommation ou une sortie contrôlée du stock.", "Enregistrer la sortie", false);
            return View(new StockMovementFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Output(StockMovementFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateArticlesAsync(model.ArticleId);
                ConfigureView("Sortie de stock", "Enregistrez une consommation ou une sortie contrôlée du stock.", "Enregistrer la sortie", false);
                return View(model);
            }

            try
            {
                await _stockMovementService.RegisterOutputAsync(model, GetActor(), GetUserId());
                TempData["SuccessMessage"] = "Sortie de stock enregistrée avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateArticlesAsync(model.ArticleId);
                ConfigureView("Sortie de stock", "Enregistrez une consommation ou une sortie contrôlée du stock.", "Enregistrer la sortie", false);
                return View(model);
            }
        }

        public async Task<IActionResult> Return()
        {
            await PopulateArticlesAsync();
            ConfigureView("Retour de stock", "Réintégrez en stock un équipement ou un lot retourné.", "Enregistrer le retour", false);
            return View(new StockMovementFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(StockMovementFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateArticlesAsync(model.ArticleId);
                ConfigureView("Retour de stock", "Réintégrez en stock un équipement ou un lot retourné.", "Enregistrer le retour", false);
                return View(model);
            }

            try
            {
                await _stockMovementService.RegisterReturnAsync(model, GetActor(), GetUserId());
                TempData["SuccessMessage"] = "Retour de stock enregistré avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateArticlesAsync(model.ArticleId);
                ConfigureView("Retour de stock", "Réintégrez en stock un équipement ou un lot retourné.", "Enregistrer le retour", false);
                return View(model);
            }
        }

        private async Task PopulateArticlesAsync(int? articleId = null)
        {
            var articles = await _stockMovementService.GetArticlesAsync();
            ViewData["ArticleId"] = new SelectList(articles, "ArticleId", "Name", articleId);
        }

        private void ConfigureView(string title, string description, string submitLabel, bool showReplenishment)
        {
            ViewBag.MovementTitle = title;
            ViewBag.MovementDescription = description;
            ViewBag.SubmitLabel = submitLabel;
            ViewBag.ShowReplenishment = showReplenishment;
        }

        private string GetActor() => HttpContext.Session.GetString("Username") ?? "system";
        private int? GetUserId() => HttpContext.Session.GetInt32("UserId");
    }
}

