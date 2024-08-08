using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interface;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ApplicationDBContext _dBContext;

        public RaceRepository(ApplicationDBContext dBContext)
        {
            _dBContext = dBContext;
        }
        public bool Add(Race race)
        {
            _dBContext.Add(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            _dBContext.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAllRacesAsync()
        {
            return await _dBContext.Races.ToListAsync();
        }

        public async Task<IEnumerable<Race>> GetRaceByCity(string city)
        {
            return await _dBContext.Races.Where( r => r.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<Race> GetRaceByIdAsync(string id)
        {
            return await _dBContext.Races.Include( a => a.Address ).FirstOrDefaultAsync(r => r.Id.ToString().Equals(id));
        }
        public async Task<Race> GetRaceByIdAsyncNoTracking(string id)
        {
            return await _dBContext.Races.Include(a => a.Address)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id.ToString().Equals(id));
        }

        public bool Save()
        {
            var result = _dBContext.SaveChanges();
            return result > 0 ? true : false;   
        }

        public bool Update(Race race)
        {
            _dBContext.Update(race);
            return Save();
        }
    }
}
