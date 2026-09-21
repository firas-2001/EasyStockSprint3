using EasyStock.Models;
using EasyStock.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyStock.Controllers
{
    [TypeFilter(typeof(SessionAuthFilter), Arguments = new object[] { "Gestionnaire,Operateur" })]
    public class FournissuersController : Controller
    {
        private readonly FournisseurService _fournisseurService;

        public FournissuersController(FournisseurService fournisseurService)
        {
            _fournisseurService = fournisseurService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _fournisseurService.GetListeFournissuer());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fournisseur = await _fournisseurService.GetFournissuerById(id);
            if (fournisseur == null)
            {
                return NotFound();
            }

            return View(fournisseur);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Fournisseur fournisseur)
        {
            if (!ModelState.IsValid)
            {
                return View(fournisseur);
            }

            fournisseur.CreatedBy = GetActor();
            fournisseur.UpdatedBy = GetActor();
            fournisseur.CreatedAt = DateTime.UtcNow;
            fournisseur.UpdatedAt = DateTime.UtcNow;

            await _fournisseurService.AddFournissuer(fournisseur);
            TempData["SuccessMessage"] = "Fournisseur ajouté avec succès.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fournisseur = await _fournisseurService.GetFournissuerById(id);
            if (fournisseur == null)
            {
                return NotFound();
            }

            return View(fournisseur);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Fournisseur fournisseur)
        {
            if (id != fournisseur.FournisseurId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(fournisseur);
            }

            var existing = await _fournisseurService.GetFournissuerById(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Nom = fournisseur.Nom;
            existing.ContactEmail = fournisseur.ContactEmail;
            existing.PhoneNumber = fournisseur.PhoneNumber;
            existing.Address = fournisseur.Address;
            existing.UpdatedBy = GetActor();
            existing.UpdatedAt = DateTime.UtcNow;

            await _fournisseurService.UpdateFournissuer(existing);
            TempData["SuccessMessage"] = "Fournisseur modifié avec succès.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fournisseur = await _fournisseurService.GetFournissuerById(id);
            if (fournisseur == null)
            {
                return NotFound();
            }

            return View(fournisseur);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fournisseur = await _fournisseurService.GetFournissuerById(id);
            if (fournisseur == null)
            {
                return NotFound();
            }

            try
            {
                await _fournisseurService.DeleteFournissuer(fournisseur);
                TempData["SuccessMessage"] = "Fournisseur supprimé avec succès.";
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "Suppression impossible: ce fournisseur est lié à des équipements.";
            }

            return RedirectToAction(nameof(Index));
        }

        private string GetActor()
        {
            return HttpContext.Session.GetString("Username") ?? "system";
        }
    }
}



