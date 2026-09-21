using EasyStock.Models;
using EasyStock.Models.ViewModels;
using EasyStock.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EasyStock.Controllers
{
    [TypeFilter(typeof(SessionAuthFilter), Arguments = new object[] { "Gestionnaire" })]
    public class ReportsController : Controller
    {
        private readonly ReportService _reportService;
        private readonly ExportService _exportService;
        private readonly HistoryService _historyService;

        public ReportsController(ReportService reportService, ExportService exportService, HistoryService historyService)
        {
            _reportService = reportService;
            _exportService = exportService;
            _historyService = historyService;
        }

        public async Task<IActionResult> Index()
        {
            await PopulateFiltersAsync();
            return View(new ReportIndexViewModel());
        }

        public async Task<IActionResult> ExportInventory(string format = "csv")
        {
            var dataset = await _reportService.BuildInventoryReportAsync();
            return BuildFileResult(dataset, format);
        }

        public async Task<IActionResult> ExportLowStock(string format = "csv")
        {
            var dataset = await _reportService.BuildLowStockReportAsync();
            return BuildFileResult(dataset, format);
        }

        public async Task<IActionResult> ExportMovements(ReportIndexViewModel model)
        {
            var dataset = await _reportService.BuildMovementReportAsync(new StockMovementHistoryViewModel
            {
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                ArticleId = model.ArticleId,
                TypeMouvement = model.TypeMouvement,
                Username = model.Username
            });

            return BuildFileResult(dataset, model.Format);
        }

        public async Task<IActionResult> ExportReplenishments(ReportIndexViewModel model)
        {
            var dataset = await _reportService.BuildReplenishmentReportAsync(new StockMovementHistoryViewModel
            {
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                ArticleId = model.ArticleId,
                Username = model.Username
            });

            return BuildFileResult(dataset, model.Format);
        }

        private IActionResult BuildFileResult(ReportDataset dataset, string? format)
        {
            var normalizedFormat = (format ?? "csv").Trim().ToLowerInvariant();
            return normalizedFormat switch
            {
                "excel" => File(_exportService.ExportExcel(dataset), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{dataset.FileName}.xlsx"),
                "pdf" => File(_exportService.ExportPdf(dataset), "application/pdf", $"{dataset.FileName}.pdf"),
                _ => File(_exportService.ExportCsv(dataset), "text/csv", $"{dataset.FileName}.csv")
            };
        }

        private async Task PopulateFiltersAsync(int? articleId = null)
        {
            var articles = await _historyService.GetArticlesAsync();
            ViewData["ArticleId"] = new SelectList(articles, "ArticleId", "Name", articleId);
            ViewBag.TypeMouvements = MouvementType.All;
        }
    }
}

