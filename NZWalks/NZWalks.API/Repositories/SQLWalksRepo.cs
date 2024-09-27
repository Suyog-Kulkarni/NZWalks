using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalksRepo : IWalksRepo
    {
        private readonly NZWalksDbContext _dbContext;

        public SQLWalksRepo(NZWalksDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _dbContext.Walks.AddAsync(walk);
            await _dbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<List<Walk>> GetAllAsync() => await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var Walk = await _dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);

            return Walk;
        }

        public async Task<Walk?> UpdateAsync([FromRoute]Guid id, Walk walk)
        {
            var WalkDB = await _dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id==id);
            if (WalkDB is null) {
                return null;
            }
            WalkDB.Description= walk.Description;
            WalkDB.Name= walk.Name;
            WalkDB.LengthInKm= walk.LengthInKm;
            WalkDB.WalkImageUrl= walk.WalkImageUrl;
            WalkDB.RegionId= walk.RegionId;
            WalkDB.DifficultyId= walk.DifficultyId;

            await _dbContext.SaveChangesAsync();
            return WalkDB;

        }
    }
}
    