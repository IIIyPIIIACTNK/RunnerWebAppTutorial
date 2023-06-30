﻿using RunWebAppTutorial.Models;

namespace RunWebAppTutorial.Interfaces
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetAll();
        Task<Race> GetByIdAsync(int id);
        Task<Race> GetByIdNoTraking(int id);

        Task<IEnumerable<Race>> GetRaceByCity(string city);
        bool Add(Race race);
        bool Update(Race race);
        bool Delete(Race race);
        bool Save();
    }
}
