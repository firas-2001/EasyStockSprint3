using EasyStock.Data;
using EasyStock.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyStock.Service
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAll()
            => await _context.Category.OrderBy(c => c.Name).ToListAsync();

        public async Task<Category?> GetById(int? id)
            => await _context.Category.FindAsync(id);

        public async Task Add(Category category)
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Category category)
        {
            _context.Category.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Category category)
        {
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
        }

        public bool Exists(int id) => _context.Category.Any(c => c.CategoryId == id);
    }
}
