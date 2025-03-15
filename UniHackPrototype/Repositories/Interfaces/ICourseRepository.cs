using UniHack.Models;
using UniHack.Models;

namespace UniHack.Repositories.Interfaces
{
	public interface ICourseRepository
	{
		Task<List<Course>> GetAllAsync();
		Task<Course?> GetByIdAsync(Guid id);
		Task<List<Course>> GetByNameAsync(string name);
		Task<List<Course>> GetByTagAsync(Tag tag);
		Task<List<Course>> GetByCourseIdAsync(Guid memberId);
		Task<bool> AddAsync(Course course);
		Task<bool> UpdateAsync(Course course);
		Task<bool> DeleteAsync(Guid id);
		Task<bool> AddCourseAsync(Guid courseId, Guid userId);
		Task<bool> RemoveCourseAsync(Guid courseId, Guid userId);
	}
}