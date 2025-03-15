using MyAspNetVueApp.Models;
using System;
using System.Collections.Generic;

namespace UniHack.Services.Interfaces
{
	public interface IUserService
	{
		List<User> GetAllUsers();
		User? GetUserById(Guid id);
		User? GetUserByEmail(string email);
		bool CreateUser(User user);
		bool UpdateUser(User user);
		bool DeleteUser(Guid id);
		bool AuthenticateUser(string email, string password);
		bool UpdateUserProfile(Guid id, string? name, string? bio, string? university, string? degree, string? imagePath, IEnumerable<string>? tags);
		bool UpdateUserPassword(Guid id, string currentPassword, string newPassword);
		bool AddUserTag(Guid id, string tag);
		bool RemoveUserTag(Guid id, string tag);
	}
}