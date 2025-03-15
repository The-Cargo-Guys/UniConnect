using System;
using System.Collections.Generic;
using UniHackPrototype.Models;

namespace UniHack.Services.Interfaces
{
	public interface ISocietyService
	{
		List<Society> GetAllSocieties();
		Society? GetSocietyById(Guid id);
		List<Society> GetSocietiesByName(string name);
		List<Society> GetSocietiesByTag(Tag tag);
		bool CreateSociety(string name, string description, string imagePath, IEnumerable<Tag> tags);
	}
}