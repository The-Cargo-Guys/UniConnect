using System;
using System.Collections.Generic;
using UniHackPrototype.Models;

namespace UniHack.Services.Interfaces
{
	public interface ICourseService
	{
		List<Course> GetAllCourses();
		Course? GetCourseById(Guid id);
		List<Course> GetCoursesByName(string name);
		List<Course> GetCoursesByTag(Tag tag);
		List<Course> GetCoursesByMember(Guid memberId);
		bool CreateCourse(string name, string description, string imagePath, IEnumerable<Tag> tags);
	}
}