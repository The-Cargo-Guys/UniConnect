using MyAspNetVueApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UniHack.Models;
using UniHack.Repositories;
using UniHack.Repositories.Interfaces;
using UniHack.Services.Interfaces;

namespace UniHack.Services.Services
{
	public class UserService : IUserService
	{
		readonly IUserRepository _userRepository;

		public UserService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public List<User> GetAllUsers()
		{
			return _userRepository.GetAllAsync().Result;
		}

		public User? GetUserById(Guid id)
		{
			return _userRepository.GetByIdAsync(id).Result;
		}

		public User? GetUserByEmail(string email)
		{
			return _userRepository.GetByEmailAsync(email).Result;
		}

		public bool CreateUser(User user)
		{
			if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
			{
				return false;
			}

			var existingUser = _userRepository.GetByEmailAsync(user.Email).Result;
			if (existingUser != null)
			{
				return false;
			}

			user.Password = HashPassword(user.Password);
			user.Id = Guid.NewGuid();

			return _userRepository.AddAsync(user).Result;
		}

		public bool UpdateUser(User user)
		{
			return _userRepository.UpdateAsync(user).Result;
		}

		public bool DeleteUser(Guid id)
		{
			return _userRepository.DeleteAsync(id).Result;
		}

		public bool AuthenticateUser(string email, string password)
		{
			var user = _userRepository.GetByEmailAsync(email).Result;
			if (user == null)
			{
				return false;
			}

			string hashedPassword = HashPassword(password);
			return user.Password == hashedPassword;
		}

		public bool UpdateUserProfile(Guid id, string? name, string? bio, string? university,
			string? degree, string? imagePath, IEnumerable<Tag>? tags)
		{
			var user = _userRepository.GetByIdAsync(id).Result;
			if (user == null)
			{
				return false;
			}

			if (name != null) user.Name = name;
			if (bio != null) user.Bio = bio;
			if (university != null) user.University = university;
			if (degree != null) user.Degree = degree;
			if (imagePath != null) user.ImagePath = imagePath;
			if (tags != null) user.Tags = [.. tags];

			return _userRepository.UpdateAsync(user).Result;
		}

		public bool UpdateUserPassword(Guid id, string currentPassword, string newPassword)
		{
			var user = _userRepository.GetByIdAsync(id).Result;
			if (user == null)
			{
				return false;
			}

			string hashedCurrentPassword = HashPassword(currentPassword);
			if (user.Password != hashedCurrentPassword)
			{
				return false;
			}

			user.Password = HashPassword(newPassword);
			return _userRepository.UpdateAsync(user).Result;
		}

		public bool AddUserTag(Guid id, Tag tag)
		{
			var user = _userRepository.GetByIdAsync(id).Result;
			if (user == null || string.IsNullOrWhiteSpace(tag.Value))
			{
				return false;
			}

			if (!user.Tags.Contains(tag))
			{
				user.Tags.Add(tag);
				return _userRepository.UpdateAsync(user).Result;
			}

			return true;
		}

		public bool RemoveUserTag(Guid id, Tag tag)
		{
			var user = _userRepository.GetByIdAsync(id).Result;
			if (user == null || string.IsNullOrWhiteSpace(tag.Value))
			{
				return false;
			}

			user.Tags.Remove(tag);
			return _userRepository.UpdateAsync(user).Result;
		}

		private string HashPassword(string password)
		{
			using var sha256 = SHA256.Create();
			var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
			return Convert.ToBase64String(hashedBytes);
		}
	}
}