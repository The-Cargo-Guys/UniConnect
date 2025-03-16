using System;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using MyAspNetVueApp.Models;
using MyAspNetVueApp.Data;
using UniHack.Services.Interfaces;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    // ✅ Register User (No JWT)
    public async Task<User?> RegisterUser(string name, string email, string password, string phoneNumber)
    {
        if (_context.Users.Any(u => u.Email == email))
        {
            return null; // Email already in use
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            Password = hashedPassword,
            PhoneNumber = phoneNumber
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return newUser;
    }

    // ✅ Login User (Return userId instead of JWT)
    public Guid? AuthenticateUser(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return null; // Invalid login
        }

        return user.Id; // ✅ Return userId instead of a JWT
    }
}
