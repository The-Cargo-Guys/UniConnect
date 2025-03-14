namespace UniHack.Services.Services
{
	public class SocietyService : ISocietyService
    {
        private readonly ISocietyService _societyService;

        public SocietyService(ISocietyService societyService)
        {
            _societyService = societyService;
        }
    }
}
