using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Data;

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

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filteQuery = null,
            string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 10)
        {
            var walks = _dbContext.Walks.Include("Region").Include("Difficulty");

            //Without AsQueryable() → The entire dataset is fetched from the database into memory first, and then filtering happens.
            //With AsQueryable() → The filtering is converted into an SQL query and executed in the database, reducing memory usage.

            // we used AsQueryable to make sure that the filtering is done in the database and not in memory
            // Filtering
                
            if (!String.IsNullOrWhiteSpace(filterOn) &&
                !String.IsNullOrWhiteSpace(filteQuery) &&
                filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                walks = walks.Where(u => EF.Functions.Like(u.Name, $"%{filteQuery}%"));

            }

            // sorting

            if (!String.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(u => u.Name) : walks.OrderByDescending(u => u.Name);
                }
                else if(sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(u => u.LengthInKm) : walks.OrderByDescending(u => u.LengthInKm);
                }
            }

            int skipResults = (pageNumber - 1) * pageSize;

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();

            //await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var Walk = await _dbContext.Walks.Include("Region").Include("Difficulty").FirstOrDefaultAsync(x => x.Id == id);

            return Walk;
        }

        public async Task<Walk?> UpdateAsync([FromRoute] Guid id, Walk walk)
        {
            var WalkDB = await _dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
            if (WalkDB is null)
            {
                return null;
            }
            WalkDB.Description = walk.Description;
            WalkDB.Name = walk.Name;
            WalkDB.LengthInKm = walk.LengthInKm;
            WalkDB.WalkImageUrl = walk.WalkImageUrl;
            WalkDB.RegionId = walk.RegionId;
            WalkDB.DifficultyId = walk.DifficultyId;

            await _dbContext.SaveChangesAsync();
            return WalkDB;

        }

        public async Task<Walk?> DeleteAsync([FromRoute] Guid id)
        {
            var WalkDb = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (WalkDb is null)
            {
                return null;
            }

            _dbContext.Walks.Remove(WalkDb);

            await _dbContext.SaveChangesAsync();

            return WalkDb;

        }
    }
}
