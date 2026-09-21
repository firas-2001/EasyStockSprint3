using EasyStock.Models;
using EasyStock.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EasyStock.Controllers
{
    [TypeFilter(typeof(SessionAuthFilter), Arguments = new object[] { "Admin" })]
    public class UsersController : Controller
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _userService.GetAllAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            var user = await _userService.GetByIdAsync(id);
            return user == null ? NotFound() : View(user);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateRolesAsync();
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (string.IsNullOrWhiteSpace(user.PlainPassword))
            {
                ModelState.AddModelError(nameof(EasyStock.Models.User.PlainPassword), "Le mot de passe est obligatoire.");
            }

            if (!ModelState.IsValid)
            {
                await PopulateRolesAsync(user.RoleId);
                return View(user);
            }

            try
            {
                await _userService.CreateAsync(user, GetActor());
                TempData["SuccessMessage"] = "Utilisateur ajouté avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateRolesAsync(user.RoleId);
                return View(user);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.PlainPassword = string.Empty;
            user.ConfirmPassword = string.Empty;
            await PopulateRolesAsync(user.RoleId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await PopulateRolesAsync(user.RoleId);
                return View(user);
            }

            try
            {
                await _userService.UpdateAsync(user, GetActor());
                TempData["SuccessMessage"] = "Utilisateur modifié avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateRolesAsync(user.RoleId);
                return View(user);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _userService.GetByIdAsync(id);
            return user == null ? NotFound() : View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _userService.DeleteAsync(id, HttpContext.Session.GetInt32("UserId"));
                TempData["SuccessMessage"] = "Utilisateur supprimé avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Delete), new { id });
            }
        }

        private async Task PopulateRolesAsync(int? selectedRoleId = null)
        {
            var roles = await _userService.GetRolesAsync();
            ViewData["RoleId"] = new SelectList(roles, "RoleId", "RoleName", selectedRoleId);
        }

        private string GetActor()
        {
            return HttpContext.Session.GetString("Username") ?? "system";
        }
    }
}
