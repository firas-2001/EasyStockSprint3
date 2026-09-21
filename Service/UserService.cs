using EasyStock.Data;
using EasyStock.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyStock.Service
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _authService;

        public UserService(ApplicationDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.User
                .Include(u => u.Role)
                .OrderBy(u => u.Username)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int? id)
        {
            return await _context.User
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            return await _context.Role.OrderBy(r => r.RoleId).ToListAsync();
        }

        public async Task CreateAsync(User user, string actor)
        {
            if (string.IsNullOrWhiteSpace(user.PlainPassword))
            {
                throw new InvalidOperationException("Le mot de passe est obligatoire.");
            }

            if (await _context.User.AnyAsync(u => u.Username == user.Username))
            {
                throw new InvalidOperationException("Ce nom d'utilisateur existe déjà.");
            }

            if (await _context.User.AnyAsync(u => u.Email == user.Email))
            {
                throw new InvalidOperationException("Cette adresse courriel existe déjà.");
            }

            user.PwdHash = _authService.HashPassword(user.PlainPassword);
            user.CreatedBy = actor;
            user.UpdatedBy = actor;
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User updatedUser, string actor)
        {
            var existing = await _context.User.FirstOrDefaultAsync(u => u.UserId == updatedUser.UserId);
            if (existing == null)
            {
                throw new InvalidOperationException("L'utilisateur est introuvable.");
            }

            if (await _context.User.AnyAsync(u => u.UserId != updatedUser.UserId && u.Username == updatedUser.Username))
            {
                throw new InvalidOperationException("Ce nom d'utilisateur existe déjà.");
            }

            if (await _context.User.AnyAsync(u => u.UserId != updatedUser.UserId && u.Email == updatedUser.Email))
            {
                throw new InvalidOperationException("Cette adresse courriel existe déjà.");
            }

            existing.Username = updatedUser.Username;
            existing.Email = updatedUser.Email;
            existing.RoleId = updatedUser.RoleId;
            existing.UpdatedBy = actor;
            existing.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(updatedUser.PlainPassword))
            {
                existing.PwdHash = _authService.HashPassword(updatedUser.PlainPassword);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, int? currentUserId = null)
        {
            if (currentUserId.HasValue && currentUserId.Value == id)
            {
                throw new InvalidOperationException("Vous ne pouvez pas supprimer votre propre compte connecté.");
            }

            var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
            {
                throw new InvalidOperationException("L'utilisateur est introuvable.");
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
