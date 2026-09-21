using EasyStock.Data;
using EasyStock.Models;
using EasyStock.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EasyStock.Service
{
    public class StockMovementService
    {
        private readonly ApplicationDbContext _context;
        private readonly NotificationService _notificationService;

        public StockMovementService(ApplicationDbContext context, NotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<List<MouvementStock>> GetRecentAsync(int take = 25)
        {
            return await BaseQuery()
                .OrderByDescending(m => m.CreatedAt)
                .Take(take)
                .ToListAsync();
        }

        public async Task<List<Article>> GetArticlesAsync()
        {
            return await _context.Article
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public Task<MouvementStock> RegisterEntryAsync(StockMovementFormViewModel model, string actor, int? userId)
            => RegisterAsync(MouvementType.Entree, model, actor, userId);

        public Task<MouvementStock> RegisterOutputAsync(StockMovementFormViewModel model, string actor, int? userId)
            => RegisterAsync(MouvementType.Sortie, model, actor, userId);

        public Task<MouvementStock> RegisterReturnAsync(StockMovementFormViewModel model, string actor, int? userId)
            => RegisterAsync(MouvementType.Retour, model, actor, userId);

        private async Task<MouvementStock> RegisterAsync(string movementType, StockMovementFormViewModel model, string actor, int? userId)
        {
            if (model.ArticleId == null)
            {
                throw new InvalidOperationException("L'équipement sélectionné est invalide.");
            }

            var article = await _context.Article.FirstOrDefaultAsync(a => a.ArticleId == model.ArticleId.Value);
            if (article == null)
            {
                throw new InvalidOperationException("L'équipement sélectionné est introuvable.");
            }

            var stockAvant = article.NombreArticleActuel;
            var stockApres = movementType switch
            {
                var value when value == MouvementType.Sortie => stockAvant - model.Quantite,
                _ => stockAvant + model.Quantite
            };

            if (movementType == MouvementType.Sortie && stockAvant < model.Quantite)
            {
                throw new InvalidOperationException("Stock insuffisant pour enregistrer cette sortie.");
            }

            var movement = new MouvementStock
            {
                ReferenceCode = await BuildReferenceAsync(movementType),
                ArticleId = article.ArticleId,
                UserId = userId,
                TypeMouvement = movementType,
                Quantite = model.Quantite,
                StockAvant = stockAvant,
                StockApres = stockApres,
                ReferenceExterne = model.ReferenceExterne,
                Motif = model.Motif,
                EstReapprovisionnement = movementType == MouvementType.Entree && model.EstReapprovisionnement,
                AlerteStockFaibleDeclenchee = stockApres <= article.NombreArticleMinimum,
                CreatedBy = actor,
                CreatedAt = DateTime.UtcNow
            };

            article.NombreArticleActuel = stockApres;
            article.UpdatedBy = actor;
            article.UpdatedAt = DateTime.UtcNow;

            _context.MouvementStock.Add(movement);
            await _context.SaveChangesAsync();

            if (movement.AlerteStockFaibleDeclenchee)
            {
                await _notificationService.SendLowStockAlertAsync(article, actor);
            }

            return movement;
        }

        private async Task<string> BuildReferenceAsync(string movementType)
        {
            var sequence = await _context.MouvementStock.CountAsync() + 1;
            var prefix = movementType switch
            {
                var value when value == MouvementType.Entree => "ENT",
                var value when value == MouvementType.Sortie => "SOR",
                var value when value == MouvementType.Retour => "RET",
                _ => "MVT"
            };

            return $"{prefix}-{DateTime.UtcNow:yyyyMMdd}-{sequence:D5}";
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
