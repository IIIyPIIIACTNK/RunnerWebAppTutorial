using RunWebAppTutorial.Data;
using RunWebAppTutorial.Interfaces;
using RunWebAppTutorial.Models;

namespace RunWebAppTutorial.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(ApplicationDBContext context,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Club>> GetAllUserClubs()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.GetUserById();
            var userClubs = _context.Clubs.Where(c => c.AppUserId == currentUser).ToList();
            return userClubs;
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.GetUserById();
            var userRaces = _context.Races.Where(c => c.AppUserId == currentUser).ToList();
            return userRaces;
        }
    }
}
