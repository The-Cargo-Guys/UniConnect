using MyAspNetVueApp.Models;
using System;
using System.Threading.Tasks;

namespace UniHack.Services.Interfaces
{
    public interface IAuthService
    {
        Guid? AuthenticateUser(string email, string password); // ✅ Return userId (Guid) instead of JWT

        Task<User?> RegisterUser(string name, string email, string password, string phoneNumber);
    }
}
