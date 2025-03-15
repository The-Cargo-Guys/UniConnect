using System;
using System.Collections.Generic;
using UniHackPrototype.Models;

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
		bool UpdateUserProfile(Guid id, string? name, string? bio, string? university, string? degree, string? imagePath, IEnumerable<Tag>? tags);
		bool UpdateUserPassword(Guid id, string currentPassword, string newPassword);
		bool AddUserTag(Guid id, Tag tag);
		bool RemoveUserTag(Guid id, Tag tag);
	}
}