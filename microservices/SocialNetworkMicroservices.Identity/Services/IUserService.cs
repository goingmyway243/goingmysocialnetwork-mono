using SocialNetworkMicroservices.Identity.Models;

namespace SocialNetworkMicroservices.Identity.Services;

public interface IUserService
{
    Task<ApplicationUser?> ValidateCredentialsAsync(string username, string password);
    Task<ApplicationUser?> GetUserByUsernameAsync(string username);
    Task<ApplicationUser?> GetUserByIdAsync(string userId);
    Task<ApplicationUser> CreateUserAsync(string username, string password, string email, string firstName, string lastName, List<string> roles);
    Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
}
