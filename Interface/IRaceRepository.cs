using RunGroopWebApp.Models;

namespace RunGroopWebApp.Interface
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetAllRacesAsync();
        Task<Race> GetRaceByIdAsync(string id);
        Task<Race> GetRaceByIdAsyncNoTracking(string id);
        Task<IEnumerable<Race>> GetRaceByCity(string city);
        bool Add(Race race);
        bool Update(Race race);
        bool Delete(Race race);
        bool Save();
    }
}
