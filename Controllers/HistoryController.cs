using EasyStock.Models;
using EasyStock.Models.ViewModels;
using EasyStock.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EasyStock.Controllers
{
    [TypeFilter(typeof(SessionAuthFilter), Arguments = new object[] { "Gestionnaire" })]
    public class HistoryController : Controller
    {
        private readonly HistoryService _historyService;

        public HistoryController(HistoryService historyService)
        {
            _historyService = historyService;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, int? articleId, string? typeMouvement, string? username)
        {
            var filter = new StockMovementHistoryViewModel
            {
                StartDate = startDate,
                EndDate = endDate,
                ArticleId = articleId,
                TypeMouvement = typeMouvement,
                Username = username
            };

            filter.Items = await _historyService.GetHistoryAsync(filter);
            await PopulateFiltersAsync(articleId);
            return View(filter);
        }

        public async Task<IActionResult> Replenishments(DateTime? startDate, DateTime? endDate, int? articleId, string? username)
        {
            var filter = new StockMovementHistoryViewModel
            {
                StartDate = startDate,
                EndDate = endDate,
                ArticleId = articleId,
                Username = username
            };

            filter.Items = await _historyService.GetReplenishmentHistoryAsync(filter);
            await PopulateFiltersAsync(articleId);
            return View(filter);
        }

        private async Task PopulateFiltersAsync(int? articleId)
        {
            var articles = await _historyService.GetArticlesAsync();
            ViewData["ArticleId"] = new SelectList(articles, "ArticleId", "Name", articleId);
            ViewBag.TypeMouvements = MouvementType.All;
        }
    }
}

