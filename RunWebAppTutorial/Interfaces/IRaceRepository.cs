using RunWebAppTutorial.Models;

namespace RunWebAppTutorial.Interfaces
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetAll();
        Task<Race> GetById(int id);

        Task<IEnumerable<Race>> GetRaceByCity(string city);
        bool Add(Race race);
        bool Update(Race race);
        bool Delete(Race race);
        bool Save();
    }
}
