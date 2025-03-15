using UniHack.Repositories;
using UniHack.Repositories.Interfaces;
using UniHack.Services.Interfaces;
using UniHackPrototype.Models;

namespace UniHack.Services
{
	public class SocietyService : ISocietyService
	{
		private readonly ISocietyRepository _societyRepository;
		private readonly IUserRepository _userRepository;

		public SocietyService(ISocietyRepository societyRepository, IUserRepository userRepository)
		{
			_societyRepository = societyRepository;
			_userRepository = userRepository;
		}

		public List<Society> GetAllSocieties()
		{
			return _societyRepository.GetAllAsync().Result;
		}

		public Society? GetSocietyById(Guid id)
		{
			return _societyRepository.GetByIdAsync(id).Result;
		}

		public List<Society> GetSocietiesByName(string name)
		{
			return _societyRepository.GetByNameAsync(name).Result;
		}

		public List<Society> GetSocietiesByTag(Tag tag)
		{
			return _societyRepository.GetByTagAsync(tag).Result;
		}

		public bool CreateSociety(string name, string description, string imagePath, IEnumerable<Tag> tags)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return false;
			}

			var society = new Society
			{
				Id = Guid.NewGuid(),
				Name = name,
				Description = description ?? string.Empty,
				ImagePathBanner = imagePath ?? string.Empty,
				Tags = tags?.ToList() ?? new List<Tag>(),
				Members = []
			};

			return _societyRepository.AddAsync(society).Result;
		}

		public bool UpdateSociety(Guid id, string? name, string? description, string? imagePath, IEnumerable<Tag>? tags)
		{
			var society = _societyRepository.GetByIdAsync(id).Result;
			if (society == null)
			{
				return false;
			}

			if (name != null) society.Name = name;
			if (description != null) society.Description = description;
			if (imagePath != null) society.ImagePathBanner = imagePath;
			if (tags != null) society.Tags = [.. tags];

			return _societyRepository.UpdateAsync(society).Result;
		}

		public bool DeleteSociety(Guid id)
		{
			return _societyRepository.DeleteAsync(id).Result;
		}

		public bool AddMemberToSociety(Guid societyId, Guid userId)
		{
			var user = _userRepository.GetByIdAsync(userId).Result;
			if (user == null)
			{
				return false;
			}

			return _societyRepository.AddMemberAsync(societyId, userId).Result;
		}

		public bool RemoveMemberFromSociety(Guid societyId, Guid userId)
		{
			return _societyRepository.RemoveMemberAsync(societyId, userId).Result;
		}

		public bool AddTag(Guid id, Tag tag)
		{
			var society = _societyRepository.GetByIdAsync(id).Result;
			if (society == null || string.IsNullOrWhiteSpace(tag.Value))
			{
				return false;
			}

			if (!society.Tags.Contains(tag))
			{
				society.Tags.Add(tag);
				return _societyRepository.UpdateAsync(society).Result;
			}

			return true;
		}

		public bool RemoveTag(Guid id, Tag tag)
		{
			var society = _societyRepository.GetByIdAsync(id).Result;
			if (society == null || string.IsNullOrWhiteSpace(tag.Value))
			{
				return false;
			}

			if (society.Tags.Contains(tag))
			{
				society.Tags.Remove(tag);
				return _societyRepository.UpdateAsync(society).Result;
			}

			return true;
		}
	}
}