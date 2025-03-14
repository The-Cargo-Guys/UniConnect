namespace UniHack.Services.Services
{
	public class CourseService : ICourseService
    {
        private readonly ICourseService _courseService;
        public CourseService(ICourseService courseService)
        {
            _courseService = courseService;
        }
    }
}
