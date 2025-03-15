using MyAspNetVueApp.Models;
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
		List<Course> GetCoursesByTag(string tag);
		List<Course> GetCoursesByMember(Guid memberId);

		bool CreateCourse(string name, string description, string imagePath, IEnumerable<string> tags);
		bool UpdateCourse(Guid id, string? name, string? description, string? imagePath, IEnumerable<string>? tags);
		bool DeleteCourse(Guid id);

		bool AddMemberToCourse(Guid courseId, Guid userId);
		bool RemoveMemberFromCourse(Guid courseId, Guid userId);

		bool AddCourseTag(Guid id, string tag);
		bool RemoveCommunityTag(Guid id, string tag);
	}
}