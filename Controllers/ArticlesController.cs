using EasyStock.Models;
using EasyStock.Service;
using Microsoft.AspNetCore.Mvc;

namespace EasyStock.Controllers
{
    [TypeFilter(typeof(SessionAuthFilter), Arguments = new object[] { "Gestionnaire,Operateur" })]
    public class ArticlesController : Controller
    {
        private readonly ArticleService _articleService;
        private readonly FournisseurService _fournisseurService;

        public ArticlesController(ArticleService articleService, FournisseurService fournisseurService)
        {
            _articleService = articleService;
            _fournisseurService = fournisseurService;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetListeArticle();
            return View(articles);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["FournisseurId"] = await _fournisseurService.GetFournissuersForArticle();
            ViewData["CategoryId"] = await _articleService.GetCategoriesSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article)
        {
            if (!ModelState.IsValid)
            {
                ViewData["FournisseurId"] = await _fournisseurService.GetFournissuersForArticle();
                ViewData["CategoryId"] = await _articleService.GetCategoriesSelectList();
                return View(article);
            }

            article.CreatedBy = GetActor();
            article.UpdatedBy = GetActor();
            article.CreatedAt = DateTime.UtcNow;
            article.UpdatedAt = DateTime.UtcNow;
            await _articleService.AddArticle(article);

            TempData["SuccessMessage"] = "Équipement ajouté avec succès.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _articleService.GetArticleById(id);
            if (article == null)
            {
                return NotFound();
            }

            ViewData["FournisseurId"] = await _fournisseurService.GetFournissuersForArticle();
            ViewData["CategoryId"] = await _articleService.GetCategoriesSelectList();
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Article article)
        {
            if (id != article.ArticleId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewData["FournisseurId"] = await _fournisseurService.GetFournissuersForArticle();
                ViewData["CategoryId"] = await _articleService.GetCategoriesSelectList();
                return View(article);
            }

            var existing = await _articleService.GetArticleById(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = article.Name;
            existing.Description = article.Description;
            existing.FournisseurId = article.FournisseurId;
            existing.CategoryId = article.CategoryId;
            existing.NombreArticleActuel = article.NombreArticleActuel;
            existing.NombreArticleMinimum = article.NombreArticleMinimum;
            existing.UpdatedBy = GetActor();
            existing.UpdatedAt = DateTime.UtcNow;

            await _articleService.UpdateArticle(existing);
            TempData["SuccessMessage"] = "Équipement modifié avec succès.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _articleService.GetArticleById(id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _articleService.GetArticleById(id);
            if (article == null)
            {
                return NotFound();
            }

            await _articleService.DeleteArticle(article);
            TempData["SuccessMessage"] = "Équipement supprimé avec succès.";
            return RedirectToAction(nameof(Index));
        }

        private string GetActor()
        {
            return HttpContext.Session.GetString("Username") ?? "system";
        }
    }
}



