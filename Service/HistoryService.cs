using EasyStock.Data;
using EasyStock.Models;
using EasyStock.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EasyStock.Service
{
    public class HistoryService
    {
        private readonly ApplicationDbContext _context;

        public HistoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MouvementStock>> GetHistoryAsync(StockMovementHistoryViewModel filter)
        {
            var query = ApplyFilters(BaseQuery(), filter);
            return await query.OrderByDescending(m => m.CreatedAt).ToListAsync();
        }

        public async Task<List<MouvementStock>> GetReplenishmentHistoryAsync(StockMovementHistoryViewModel filter)
        {
            var query = ApplyFilters(BaseQuery(), filter)
                .Where(m => m.EstReapprovisionnement || m.TypeMouvement == MouvementType.Entree);

            return await query.OrderByDescending(m => m.CreatedAt).ToListAsync();
        }

        public async Task<List<Article>> GetArticlesAsync()
        {
            return await _context.Article.OrderBy(a => a.Name).ToListAsync();
        }

        private IQueryable<MouvementStock> ApplyFilters(IQueryable<MouvementStock> query, StockMovementHistoryViewModel filter)
        {
            if (filter.StartDate.HasValue)
            {
                var startUtc = ConvertLocalDateBoundaryToUtc(filter.StartDate.Value.Date, isEndOfDay: false);
                query = query.Where(m => m.CreatedAt >= startUtc);
            }

            if (filter.EndDate.HasValue)
            {
                var endLocal = filter.EndDate.Value.Date.AddDays(1).AddTicks(-1);
                var endUtc = ConvertLocalDateBoundaryToUtc(endLocal, isEndOfDay: true);
                query = query.Where(m => m.CreatedAt <= endUtc);
            }

            if (filter.ArticleId.HasValue)
            {
                query = query.Where(m => m.ArticleId == filter.ArticleId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.TypeMouvement))
            {
                query = query.Where(m => m.TypeMouvement == filter.TypeMouvement);
            }

            if (!string.IsNullOrWhiteSpace(filter.Username))
            {
                var username = filter.Username.Trim();
                query = query.Where(m => m.CreatedBy.Contains(username) || (m.User != null && m.User.Username.Contains(username)));
            }

            return query;
        }

        private static DateTime ConvertLocalDateBoundaryToUtc(DateTime localDateTime, bool isEndOfDay)
        {
            var unspecifiedLocal = DateTime.SpecifyKind(localDateTime, DateTimeKind.Unspecified);
            var localZone = TimeZoneInfo.Local;
            var converted = TimeZoneInfo.ConvertTimeToUtc(unspecifiedLocal, localZone);

            if (!isEndOfDay)
            {
                return converted;
            }

            return converted;
        }

        private IQueryable<MouvementStock> BaseQuery()
        {
            return _context.MouvementStock
                .Include(m => m.Article)
                    .ThenInclude(a => a!.Category)
                .Include(m => m.Article)
                    .ThenInclude(a => a!.Fournisseur)
                .Include(m => m.User);
        }
    }
}
