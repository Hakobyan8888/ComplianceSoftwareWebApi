using ComplianceSoftwareWebApi.Data;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ComplianceSoftwareWebApi.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public string GetUserIdFromClaims(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userIdClaim;
        }

        public async Task<bool> HasPermissionAsync(string userId, PermissionTypes permissionTypes)
        {
            var permission = await _context.Permissions.FirstOrDefaultAsync(x => x.Name == permissionTypes);
            if (permission == null)
            {
                return false;
            }
            // Assuming you have a way to check permissions in your database
            var userPermissions = await _context.UserPermissions
                .Where(up => up.UserId == userId)
                .Select(up => up.Permission)
                .ToListAsync();

            return userPermissions.Contains(permission);
        }
    }

}
