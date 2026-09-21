using System.Security.Cryptography;
using System.Text;
using EasyStock.Data;
using EasyStock.Models;
using EasyStock.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EasyStock.Service
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> ValidateCredentialsAsync(string username, string password)
        {
            var user = await _context.User
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return null;
            }

            return VerifyPassword(password, user.PwdHash) ? user : null;
        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }

        public bool VerifyPassword(string password, string storedHash)
        {
            return string.Equals(HashPassword(password), storedHash, StringComparison.OrdinalIgnoreCase);
        }

        public void SignIn(HttpContext httpContext, User user)
        {
            httpContext.Session.SetInt32("UserId", user.UserId);
            httpContext.Session.SetString("Username", user.Username);
            httpContext.Session.SetString("Role", user.Role?.RoleName ?? string.Empty);
            httpContext.Session.SetString("IsAuthenticated", "true");
        }

        public void SignOut(HttpContext httpContext)
        {
            httpContext.Session.Clear();
        }
    }
}
