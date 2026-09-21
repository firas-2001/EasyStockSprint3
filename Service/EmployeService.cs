using EasyStock.Data;
using EasyStock.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyStock.Service
{
    public class EmployeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employe>> GetActifsAsync(string? searchField = null, string? searchTerm = null)
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

        public async Task<Employe?> GetByIdAsync(int? id)
        {
            return await _context.Employe.FirstOrDefaultAsync(e => e.EmployeId == id);
        }
    }
}
