using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interface;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository
{
    public class ClubRepository : IClubRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public ClubRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool Add(Club club)
        {
            _dbContext.Add(club);
            return Save();
                
        }

        public bool Delete(Club club)
        {
            _dbContext.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAllClubsAsync()
        {
            return  await _dbContext.Clubs.ToListAsync();
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
           return  await _dbContext.Clubs.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<Club> GetClubByIdAsync(string id)
        {
            return await _dbContext.Clubs.Include(a =>a.Address)
                        .FirstOrDefaultAsync(c => c.Id.ToString().Equals(id));
        }
        public async Task<Club> GetClubByIdAsyncNoTracking(string id)
        {
            return await _dbContext.Clubs.Include(a => a.Address)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id.ToString()
                .Equals(id));
        }

        public bool Save()
        {
           
            return _dbContext.SaveChanges() > 0 ? true : false;
        }

        public bool Update(Club club)
        {
            _dbContext.Update(club);    
            return Save();  
        }
    }
}
