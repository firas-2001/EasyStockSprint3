using EasyStock.Data;
using EasyStock.Models;
using EasyStock.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EasyStock.Service
{
    public class ReportService
    {
        private readonly ApplicationDbContext _context;
        private readonly HistoryService _historyService;

        public ReportService(ApplicationDbContext context, HistoryService historyService)
        {
            _context = context;
            _historyService = historyService;
        }

        public async Task<ReportDataset> BuildInventoryReportAsync()
        {
            var articles = await _context.Article
                .Include(a => a.Category)
                .Include(a => a.Fournisseur)
                .OrderBy(a => a.Name)
                .ToListAsync();

            return new ReportDataset
            {
                Title = "Inventaire global",
                FileName = $"inventaire-global-{DateTime.UtcNow:yyyyMMdd-HHmm}",
                WorksheetName = "Inventaire",
                Headers = new List<string> { "ID", "Équipement", "Catégorie", "Fournisseur", "Stock disponible", "Seuil minimal", "Statut" },
                Rows = articles.Select(article => (IReadOnlyList<string>)new List<string>
                {
                    article.ArticleId.ToString(),
                    article.Name,
                    article.Category?.Name ?? string.Empty,
                    article.Fournisseur?.Nom ?? string.Empty,
                    article.NombreArticleActuel.ToString(),
                    article.NombreArticleMinimum.ToString(),
                    GetStatus(article)
                }).ToList()
            };
        }

        public async Task<ReportDataset> BuildLowStockReportAsync()
        {
            var articles = await _context.Article
                .Include(a => a.Category)
                .Include(a => a.Fournisseur)
                .Where(a => a.NombreArticleActuel <= a.NombreArticleMinimum)
                .OrderBy(a => a.NombreArticleActuel)
                .ThenBy(a => a.Name)
                .ToListAsync();

            return new ReportDataset
            {
                Title = "Stock faible",
                FileName = $"stock-faible-{DateTime.UtcNow:yyyyMMdd-HHmm}",
                WorksheetName = "Stock faible",
                Headers = new List<string> { "ID", "Équipement", "Catégorie", "Fournisseur", "Stock disponible", "Seuil minimal", "Écart" },
                Rows = articles.Select(article => (IReadOnlyList<string>)new List<string>
                {
                    article.ArticleId.ToString(),
                    article.Name,
                    article.Category?.Name ?? string.Empty,
                    article.Fournisseur?.Nom ?? string.Empty,
                    article.NombreArticleActuel.ToString(),
                    article.NombreArticleMinimum.ToString(),
                    (article.NombreArticleMinimum - article.NombreArticleActuel).ToString()
                }).ToList()
            };
        }

        public async Task<ReportDataset> BuildMovementReportAsync(StockMovementHistoryViewModel filter)
        {
            var items = await _historyService.GetHistoryAsync(filter);
            return new ReportDataset
            {
                Title = "Historique des mouvements",
                FileName = $"mouvements-stock-{DateTime.UtcNow:yyyyMMdd-HHmm}",
                WorksheetName = "Mouvements",
                Headers = new List<string> { "Référence", "Date", "Type", "Équipement", "Catégorie", "Quantité", "Stock avant", "Stock après", "Utilisateur", "Référence externe", "Motif" },
                Rows = items.Select(item => (IReadOnlyList<string>)new List<string>
                {
                    item.ReferenceCode,
                    item.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                    MouvementType.ToDisplay(item.TypeMouvement),
                    item.Article?.Name ?? string.Empty,
                    item.Article?.Category?.Name ?? string.Empty,
                    item.Quantite.ToString(),
                    item.StockAvant.ToString(),
                    item.StockApres.ToString(),
                    item.CreatedBy,
                    item.ReferenceExterne ?? string.Empty,
                    item.Motif ?? string.Empty
                }).ToList()
            };
        }

        public async Task<ReportDataset> BuildReplenishmentReportAsync(StockMovementHistoryViewModel filter)
        {
            var items = await _historyService.GetReplenishmentHistoryAsync(filter);
            return new ReportDataset
            {
                Title = "Historique des réapprovisionnements",
                FileName = $"reapprovisionnements-{DateTime.UtcNow:yyyyMMdd-HHmm}",
                WorksheetName = "Réappro",
                Headers = new List<string> { "Référence", "Date", "Équipement", "Fournisseur", "Quantité", "Stock avant", "Stock après", "Utilisateur", "Référence externe", "Motif" },
                Rows = items.Select(item => (IReadOnlyList<string>)new List<string>
                {
                    item.ReferenceCode,
                    item.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                    item.Article?.Name ?? string.Empty,
                    item.Article?.Fournisseur?.Nom ?? string.Empty,
                    item.Quantite.ToString(),
                    item.StockAvant.ToString(),
                    item.StockApres.ToString(),
                    item.CreatedBy,
                    item.ReferenceExterne ?? string.Empty,
                    item.Motif ?? string.Empty
                }).ToList()
            };
        }

        private static string GetStatus(Article article)
        {
            if (article.NombreArticleActuel <= article.NombreArticleMinimum)
            {
                return "Stock faible";
            }

            return "Stock normal";
        }
    }
}
