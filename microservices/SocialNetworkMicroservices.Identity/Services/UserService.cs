using Microsoft.EntityFrameworkCore;
using SocialNetworkMicroservices.Identity.Data;
using SocialNetworkMicroservices.Identity.Models;

namespace SocialNetworkMicroservices.Identity.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(ApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<ApplicationUser?> ValidateCredentialsAsync(string username, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower() && u.IsActive);

        if (user == null)
        {
            return null;
        }

        return _passwordHasher.VerifyPassword(password, user.PasswordHash) ? user : null;
    }

    public async Task<ApplicationUser?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<ApplicationUser> CreateUserAsync(
        string username, 
        string password, 
        string email, 
        string firstName, 
        string lastName, 
        List<string> roles)
    {
        var existingUser = await GetUserByUsernameAsync(username);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"User with username '{username}' already exists.");
        }

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            Username = username,
            PasswordHash = _passwordHasher.HashPassword(password),
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Roles = roles,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
    {
        return await _context.Users
            .Where(u => u.IsActive)
            .ToListAsync();
    }
}
