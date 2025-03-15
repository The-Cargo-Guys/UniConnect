using MyAspNetVueApp.Models;
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
		List<Society> GetSocietiesByTag(string tag);
		bool CreateSociety(string name, string description, string imagePath, IEnumerable<string> tags);
	}
}