using EasyStock.Models.ViewModels;
using EasyStock.Service;
using Microsoft.AspNetCore.Mvc;

namespace EasyStock.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (HttpContext.Session.GetString("IsAuthenticated") == "true")
            {
                return RedirectToAction("Accueil", "Home");
            }

            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _authService.ValidateCredentialsAsync(model.Username, model.Password);
            if (user == null)
            {
                ViewData["ErrorMessage"] = "Nom d'utilisateur ou mot de passe invalide.";
                return View(model);
            }

            _authService.SignIn(HttpContext, user);

            if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction("Accueil", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            _authService.SignOut(HttpContext);
            return RedirectToAction(nameof(Login));
        }
    }
}
