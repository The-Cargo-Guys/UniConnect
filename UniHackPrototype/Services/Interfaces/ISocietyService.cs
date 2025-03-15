using MyAspNetVueApp.Models;
using System;
using System.Collections.Generic;
using UniHack.Models;

namespace UniHack.Services.Interfaces
{
	public interface ISocietyService
	{
		List<Society> GetAllSocieties();
		Society? GetSocietyById(Guid id);
		List<Society> GetSocietiesByName(string name);
		List<Society> GetSocietiesByTag(Tag tag);

		bool CreateSociety(string name, string description, string imagePath, IEnumerable<Tag> tags);
		bool UpdateSociety(Guid id, string? name, string? description, string? imagePath, IEnumerable<Tag>? tags);
		bool DeleteSociety(Guid id);

		bool AddMemberToSociety(Guid societyId, Guid userId);
		bool RemoveMemberFromSociety(Guid societyId, Guid userId);

		bool AddTag(Guid id, Tag tag);
		bool RemoveTag(Guid id, Tag tag);
	}
}