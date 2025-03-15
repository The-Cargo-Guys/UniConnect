using MyAspNetVueApp.Models;

namespace UniHack.Services.Interfaces
{
    public interface IAuthService
    {
        string? AuthenticateUser(string email, string password);

        Task<User?> RegisterUser(string name, string email, string password, string phoneNumber);

    }
}
