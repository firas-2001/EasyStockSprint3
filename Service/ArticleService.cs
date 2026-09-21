using EasyStock.Data;
using EasyStock.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EasyStock.Service
{
    public class ArticleService
    {
        private readonly ApplicationDbContext _context;

        public ArticleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Article>> GetListeArticle()
            => await _context.Article.Include(a => a.Fournisseur).Include(a => a.Category).ToListAsync();

        public async Task AddArticle(Article article)
        {
            _context.Article.Add(article);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateArticle(Article article)
        {
            _context.Article.Update(article);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteArticle(Article article)
        {
            _context.Article.Remove(article);
            await _context.SaveChangesAsync();
        }

        public async Task<Article?> GetArticleById(int? id)
            => await _context.Article.Include(a => a.Fournisseur).Include(a => a.Category).FirstOrDefaultAsync(a => a.ArticleId == id);

        public bool ArticleExists(int id) => _context.Article.Any(a => a.ArticleId == id);

        public async Task<SelectList> GetCategoriesSelectList()
        {
            var categories = await _context.Category.OrderBy(c => c.Name).ToListAsync();
            return new SelectList(categories, "CategoryId", "Name");
        }
    }
}
