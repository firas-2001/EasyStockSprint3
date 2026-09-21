using EasyStock.Models;
using EasyStock.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyStock.Controllers
{
    [TypeFilter(typeof(SessionAuthFilter), Arguments = new object[] { "Gestionnaire,Operateur" })]
    public class CategoriesController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAll();
            return View(categories);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            category.CreatedBy = GetActor();
            category.UpdatedBy = GetActor();
            category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = DateTime.UtcNow;

            await _categoryService.Add(category);
            TempData["SuccessMessage"] = "Catégorie ajoutée avec succès.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var existing = await _categoryService.GetById(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = category.Name;
            existing.UpdatedBy = GetActor();
            existing.UpdatedAt = DateTime.UtcNow;

            await _categoryService.Update(existing);
            TempData["SuccessMessage"] = "Catégorie modifiée avec succès.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            try
            {
                await _categoryService.Delete(category);
                TempData["SuccessMessage"] = "Catégorie supprimée avec succès.";
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "Suppression impossible: la catégorie est liée à des équipements.";
            }

            return RedirectToAction(nameof(Index));
        }

        private string GetActor()
        {
            return HttpContext.Session.GetString("Username") ?? "system";
        }
    }
}



