using EasyStock.Data;
using EasyStock.Models;
using EasyStock.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EasyStock.Service
{
    public class AffectationService
    {
        private readonly ApplicationDbContext _context;
        private readonly NotificationService _notificationService;

        public AffectationService(ApplicationDbContext context, NotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<List<Affectation>> GetActiveAsync(string? searchField = null, string? searchTerm = null)
        {
            return await ApplyEmployeFilter(
                    _context.Affectation
                        .Include(a => a.Article)
                        .Include(a => a.Employe)
                        .Where(a => a.IsActive),
                    searchField,
                    searchTerm)
                .OrderByDescending(a => a.AssignedAt)
                .ToListAsync();
        }

        public async Task<List<Affectation>> GetHistoryAsync(string? searchField = null, string? searchTerm = null)
        {
            return await ApplyEmployeFilter(
                    _context.Affectation
                        .Include(a => a.Article)
                        .Include(a => a.Employe),
                    searchField,
                    searchTerm)
                .OrderByDescending(a => a.AssignedAt)
                .ToListAsync();
        }

        public async Task<Affectation?> GetByIdAsync(int? id)
        {
            return await _context.Affectation
                .Include(a => a.Article)
                .Include(a => a.Employe)
                .FirstOrDefaultAsync(a => a.AffectationId == id);
        }

        public async Task<List<Article>> GetAssignableArticlesAsync()
        {
            return await _context.Article
                .Include(a => a.Category)
                .Where(a => a.NombreArticleActuel > 0)
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<List<Employe>> GetAssignableEmployesAsync(string? searchField = null, string? searchTerm = null)
        {
            var query = _context.Employe
                .Where(e => e.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var trimmed = searchTerm.Trim();
                query = (searchField ?? "Nom").ToLowerInvariant() switch
                {
                    "id" when int.TryParse(trimmed, out var employeId) => query.Where(e => e.EmployeId == employeId),
                    "prenom" => query.Where(e => e.Prenom.Contains(trimmed)),
                    _ => query.Where(e => e.Nom.Contains(trimmed))
                };
            }

            return await query
                .OrderBy(e => e.Nom)
                .ThenBy(e => e.Prenom)
                .ToListAsync();
        }

        public async Task CreateAsync(AffectationCreateViewModel model, string actor, int? userId = null)
        {
            var article = await _context.Article.FirstOrDefaultAsync(a => a.ArticleId == model.ArticleId);
            if (article == null)
            {
                throw new InvalidOperationException("L'équipement sélectionné est introuvable.");
            }

            var employe = await _context.Employe.FirstOrDefaultAsync(e => e.EmployeId == model.EmployeId && e.IsActive);
            if (employe == null)
            {
                throw new InvalidOperationException("L'employé sélectionné est introuvable ou inactif.");
            }

            if (article.NombreArticleActuel <= 0)
            {
                throw new InvalidOperationException("Cet équipement n'est plus disponible pour une affectation.");
            }

            var nextSequence = await _context.Affectation.CountAsync(a => a.ArticleId == article.ArticleId) + 1;
            var now = DateTime.UtcNow;
            var stockAvant = article.NombreArticleActuel;
            var stockApres = stockAvant - 1;
            var lowStockTriggered = stockApres <= article.NombreArticleMinimum;

            article.NombreArticleActuel = stockApres;
            article.UpdatedBy = actor;
            article.UpdatedAt = now;

            var affectation = new Affectation
            {
                ArticleId = model.ArticleId,
                EmployeId = model.EmployeId,
                EtatSortie = model.EtatSortie,
                Notes = model.Notes,
                AssignedAt = now,
                IsActive = true,
                ReferenceCode = $"AFF-{article.ArticleId:D4}-{nextSequence:D4}",
                CreatedBy = actor,
                UpdatedBy = actor,
                CreatedAt = now,
                UpdatedAt = now
            };

            _context.Affectation.Add(affectation);
            _context.MouvementStock.Add(new MouvementStock
            {
                ReferenceCode = await BuildMovementReferenceAsync(MouvementType.Sortie),
                ArticleId = article.ArticleId,
                UserId = userId,
                TypeMouvement = MouvementType.Sortie,
                Quantite = 1,
                StockAvant = stockAvant,
                StockApres = stockApres,
                ReferenceExterne = affectation.ReferenceCode,
                Motif = BuildAffectationMovementNote(employe, model.Notes),
                EstReapprovisionnement = false,
                AlerteStockFaibleDeclenchee = lowStockTriggered,
                CreatedBy = actor,
                CreatedAt = now
            });

            await _context.SaveChangesAsync();

            if (lowStockTriggered)
            {
                await _notificationService.SendLowStockAlertAsync(article, actor);
            }
        }

        public async Task ReturnAsync(int affectationId, AffectationReturnViewModel model, string actor, int? userId = null)
        {
            var affectation = await _context.Affectation
                .Include(a => a.Article)
                .Include(a => a.Employe)
                .FirstOrDefaultAsync(a => a.AffectationId == affectationId);

            if (affectation == null)
            {
                throw new InvalidOperationException("L'affectation est introuvable.");
            }

            if (!affectation.IsActive)
            {
                throw new InvalidOperationException("Cet équipement est déjà désaffecté.");
            }

            var now = DateTime.UtcNow;
            affectation.IsActive = false;
            affectation.EtatRetour = model.EtatRetour;
            affectation.ReturnedAt = now;
            affectation.UpdatedBy = actor;
            affectation.UpdatedAt = now;
            affectation.Notes = string.IsNullOrWhiteSpace(model.Notes)
                ? affectation.Notes
                : string.IsNullOrWhiteSpace(affectation.Notes)
                    ? model.Notes
                    : $"{affectation.Notes} | Retour: {model.Notes}";

            if (affectation.Article != null)
            {
                var stockAvant = affectation.Article.NombreArticleActuel;
                var stockApres = stockAvant + 1;
                affectation.Article.NombreArticleActuel = stockApres;
                affectation.Article.UpdatedBy = actor;
                affectation.Article.UpdatedAt = now;

                _context.MouvementStock.Add(new MouvementStock
                {
                    ReferenceCode = await BuildMovementReferenceAsync(MouvementType.Retour),
                    ArticleId = affectation.Article.ArticleId,
                    UserId = userId,
                    TypeMouvement = MouvementType.Retour,
                    Quantite = 1,
                    StockAvant = stockAvant,
                    StockApres = stockApres,
                    ReferenceExterne = affectation.ReferenceCode,
                    Motif = BuildReturnMovementNote(affectation.Employe, model.EtatRetour, model.Notes),
                    EstReapprovisionnement = false,
                    AlerteStockFaibleDeclenchee = stockApres <= affectation.Article.NombreArticleMinimum,
                    CreatedBy = actor,
                    CreatedAt = now
                });
            }

            await _context.SaveChangesAsync();
        }

        private async Task<string> BuildMovementReferenceAsync(string movementType)
        {
            var sequence = await _context.MouvementStock.CountAsync() + 1;
            var prefix = movementType switch
            {
                var value when value == MouvementType.Sortie => "SOR",
                var value when value == MouvementType.Retour => "RET",
                _ => "MVT"
            };

            return $"{prefix}-{DateTime.UtcNow:yyyyMMdd}-{sequence:D5}";
        }

        private static string BuildAffectationMovementNote(Employe employe, string? notes)
        {
            var baseNote = $"Sortie automatique liée à l'affectation de l'équipement à {employe.Prenom} {employe.Nom}.";
            return string.IsNullOrWhiteSpace(notes) ? baseNote : $"{baseNote} Notes affectation: {notes}";
        }

        private static string BuildReturnMovementNote(Employe? employe, string etatRetour, string? notes)
        {
            var employeLabel = employe == null ? "employé inconnu" : $"{employe.Prenom} {employe.Nom}";
            var baseNote = $"Retour automatique lié à la désaffectation de l'équipement pour {employeLabel}. État déclaré: {etatRetour}.";
            return string.IsNullOrWhiteSpace(notes) ? baseNote : $"{baseNote} Notes retour: {notes}";
        }

        private static IQueryable<Affectation> ApplyEmployeFilter(IQueryable<Affectation> query, string? searchField, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return query;
            }

            var trimmed = searchTerm.Trim();
            return (searchField ?? "Nom").ToLowerInvariant() switch
            {
                "id" when int.TryParse(trimmed, out var employeId) => query.Where(a => a.EmployeId == employeId),
                "prenom" => query.Where(a => a.Employe != null && a.Employe.Prenom.Contains(trimmed)),
                _ => query.Where(a => a.Employe != null && a.Employe.Nom.Contains(trimmed))
            };
        }
    }
}
