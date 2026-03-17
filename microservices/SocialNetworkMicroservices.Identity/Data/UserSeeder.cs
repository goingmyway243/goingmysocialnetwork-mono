using Microsoft.EntityFrameworkCore;
using SocialNetworkMicroservices.Identity.Data;
using SocialNetworkMicroservices.Identity.Models;
using SocialNetworkMicroservices.Identity.Services;

namespace SocialNetworkMicroservices.Identity.Data;

public static class UserSeeder
{
    public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        // Check if users already exist
        if (await context.Users.AnyAsync())
        {
            return; // Users already seeded
        }

        var users = new List<ApplicationUser>
        {
            new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Username = "admin",
                PasswordHash = passwordHasher.HashPassword("admin123"),
                Email = "admin@socialnetwork.com",
                FirstName = "Admin",
                LastName = "User",
                Roles = new List<string> { "admin", "user" },
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Username = "john.doe",
                PasswordHash = passwordHasher.HashPassword("password123"),
                Email = "john.doe@example.com",
                FirstName = "John",
                LastName = "Doe",
                Roles = new List<string> { "user" },
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Username = "jane.smith",
                PasswordHash = passwordHasher.HashPassword("password123"),
                Email = "jane.smith@example.com",
                FirstName = "Jane",
                LastName = "Smith",
                Roles = new List<string> { "user" },
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Username = "test",
                PasswordHash = passwordHasher.HashPassword("password"),
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User",
                Roles = new List<string> { "user" },
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Username = "moderator",
                PasswordHash = passwordHasher.HashPassword("mod123"),
                Email = "moderator@socialnetwork.com",
                FirstName = "Moderator",
                LastName = "User",
                Roles = new List<string> { "moderator", "user" },
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();
    }
}
