using RunGroopWebApp.Models;

namespace RunGroopWebApp.Interface
{
    public interface IClubRepository
    {
        Task<IEnumerable<Club>>GetAllClubsAsync();
        Task<Club> GetClubByIdAsync(string id);
        Task<IEnumerable<Club>> GetClubByCity(string city);
        Task<Club> GetClubByIdAsyncNoTracking(string id);
        bool Add(Club club);
        bool Update(Club club);
        bool Delete(Club club);
        bool Save();

    }
}
