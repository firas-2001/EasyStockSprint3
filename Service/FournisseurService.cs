using EasyStock.Data;
using EasyStock.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EasyStock.Service
{
    public class FournisseurService
    {
        private readonly ApplicationDbContext _context;

        public FournisseurService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Fournisseur>> GetListeFournissuer()
            => await _context.Fournisseur.OrderBy(f => f.Nom).ToListAsync();

        public async Task<SelectList> GetFournissuersForArticle()
        {
            var fournisseurs = await _context.Fournisseur.OrderBy(f => f.Nom).ToListAsync();
            return new SelectList(fournisseurs, "FournisseurId", "Nom");
        }

        public async Task AddFournissuer(Fournisseur fournisseur)
        {
            _context.Fournisseur.Add(fournisseur);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFournissuer(Fournisseur fournisseur)
        {
            _context.Fournisseur.Remove(fournisseur);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFournissuer(Fournisseur fournisseur)
        {
            _context.Fournisseur.Update(fournisseur);
            await _context.SaveChangesAsync();
        }

        public async Task<Fournisseur?> GetFournissuerById(int? id)
            => await _context.Fournisseur.FindAsync(id);

        public bool FournissuerExists(int id)
            => _context.Fournisseur.Any(f => f.FournisseurId == id);
    }
}
