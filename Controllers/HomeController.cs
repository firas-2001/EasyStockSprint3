using EasyStock.Models;
using EasyStock.Service;
using Microsoft.AspNetCore.Mvc;

namespace EasyStock.Controllers
{
    [TypeFilter(typeof(SessionAuthFilter), Arguments = new object[] { "Admin,Gestionnaire,Operateur" })]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Accueil");
        }

        public IActionResult Accueil()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }
    }
}
