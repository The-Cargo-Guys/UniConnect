using Microsoft.EntityFrameworkCore;
using MyAspNetVueApp.Data;
using UniHackPrototype.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniHack.Repositories.Interfaces;

namespace UniHack.Repositories
{
	public class CourseRepository : ICourseRepository
	{
		private readonly AppDbContext _context;

		public CourseRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<List<Course>> GetAllAsync()
		{
			return await _context.Courses
				.Include(c => c.Members)
				.ToListAsync();
		}

		public async Task<Course?> GetByIdAsync(Guid id)
		{
			return await _context.Courses
				.Include(c => c.Members)
				.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<List<Course>> GetByNameAsync(string name)
		{
			return await _context.Courses
				.Include(c => c.Members)
				.Where(c => c.Name.Contains(name))
				.ToListAsync();
		}

		public async Task<List<Course>> GetByTagAsync(Tag tag)
		{
			return await _context.Courses
				.Include(c => c.Members)
				.Where(c => c.Tags.Contains(tag))
				.ToListAsync();
		}

		public async Task<List<Course>> GetByCourseIdAsync(Guid memberId)
		{
			return await _context.Courses
				.Include(c => c.Members)
				.Where(c => c.Members.Any(m => m.Id == memberId))
				.ToListAsync();
		}

		public async Task<bool> AddAsync(Course course)
		{
			await _context.Courses.AddAsync(course);
			return await SaveChangesAsync();
		}

		public async Task<bool> UpdateAsync(Course course)
		{
			_context.Courses.Update(course);
			return await SaveChangesAsync();
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var course = await GetByIdAsync(id);
			if (course == null)
				return false;

			_context.Courses.Remove(course);
			return await SaveChangesAsync();
		}

		public async Task<bool> AddCourseAsync(Guid courseId, Guid userId)
		{
			var course = await GetByIdAsync(courseId);
			var user = await _context.Users.FindAsync(userId);

			if (course == null || user == null)
				return false;

			course.Members.Add(user);
			return await SaveChangesAsync();
		}

		public async Task<bool> RemoveCourseAsync(Guid courseId, Guid userId)
		{
			var course = await GetByIdAsync(courseId);

			if (course == null)
				return false;

			var user = course.Members.FirstOrDefault(m => m.Id == userId);

			if (user == null)
				return false;

			course.Members.Remove(user);
			return await SaveChangesAsync();
		}

		private async Task<bool> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}
	}
}