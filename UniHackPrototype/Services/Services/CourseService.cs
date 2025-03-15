using MyAspNetVueApp.Models;
using UniHack.Models;
using UniHack.Repositories;
using UniHack.Repositories.Interfaces;
using UniHack.Services.Interfaces;
using UniHackPrototype.Models;

namespace UniHack.Services.Services
{
	public class CourseService : ICourseService
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IUserRepository _userRepository;

		public CourseService(ICourseRepository courseRepository, IUserRepository userRepository)
		{
			_courseRepository = courseRepository;
			_userRepository = userRepository;
		}

		public List<Course> GetAllCourses()
		{
			return _courseRepository.GetAllAsync().Result;
		}

		public Course? GetCourseById(Guid id)
		{
			return _courseRepository.GetByIdAsync(id).Result;
		}

		public List<Course> GetCoursesByName(string name)
		{
			return _courseRepository.GetByNameAsync(name).Result;
		}

		public List<Course> GetCoursesByTag(Tag tag)
		{
			return _courseRepository.GetByTagAsync(tag).Result;
		}

		public List<Course> GetCoursesByMember(Guid memberId)
		{
			return _courseRepository.GetByCourseIdAsync(memberId).Result;
		}

		public bool CreateCourse(string name, string description, string imagePath, IEnumerable<Tag> tags)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return false;
			}

			var course = new Course
			{
				Id = Guid.NewGuid(),
				Name = name,
				Description = description ?? string.Empty,
				ImagePathBanner = imagePath ?? string.Empty,
				Tags = tags?.ToList() ?? [],
				Members = []
			};

			return _courseRepository.AddAsync(course).Result;
		}

		public bool UpdateCourse(Guid id, string? name, string? description, string? imagePath, IEnumerable<Tag>? tags)
		{
			var course = _courseRepository.GetByIdAsync(id).Result;
			if (course == null)
			{
				return false;
			}

			if (name != null) course.Name = name;
			if (description != null) course.Description = description;
			if (imagePath != null) course.ImagePathBanner = imagePath;
			if (tags != null) course.Tags = [.. tags];

			return _courseRepository.UpdateAsync(course).Result;
		}

		public bool DeleteCourse(Guid id)
		{
			return _courseRepository.DeleteAsync(id).Result;
		}

		public bool AddMemberToCourse(Guid communityId, Guid userId)
		{
			var user = _userRepository.GetByIdAsync(userId).Result;
			if (user == null)
			{
				return false;
			}

			return _courseRepository.AddCourseAsync(communityId, userId).Result;
		}

		public bool RemoveMemberFromCourse(Guid communityId, Guid userId)
		{
			return _courseRepository.RemoveCourseAsync(communityId, userId).Result;
		}

		public bool AddCourseTag(Guid id, Tag tag)
		{
			var course = _courseRepository.GetByIdAsync(id).Result;
			if (course == null || string.IsNullOrWhiteSpace(tag.Value))
			{
				return false;
			}

			if (!course.Tags.Contains(tag))
			{
				course.Tags.Add(tag);
				return _courseRepository.UpdateAsync(course).Result;
			}

			return true;
		}

		public bool RemoveCommunityTag(Guid id, Tag tag)
		{
			var course = _courseRepository.GetByIdAsync(id).Result;
			if (course == null || string.IsNullOrWhiteSpace(tag.Value))
			{
				return false;
			}

			if (course.Tags.Contains(tag))
			{
				course.Tags.Remove(tag);
				return _courseRepository.UpdateAsync(course).Result;
			}

			return true;
		}
	}
}