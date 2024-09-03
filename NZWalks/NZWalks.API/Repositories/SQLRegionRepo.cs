using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepo : IRegionRepo
    {
        private readonly NZWalksDbContext _context;

        public SQLRegionRepo(NZWalksDbContext context)
        {
            _context = context;
        }

        public async Task<List<Region>> GetAll() => await _context.Regions.ToListAsync();

        public async Task<Region?> GetById(Guid id)
        {
            var region = await _context.Regions.FirstOrDefaultAsync(x => x.Id == id);

            return region;
        }

        public async Task<Region> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            var regiondomain = new Region
            {
                Code = addRegionRequestDTO.Code,
                Name = addRegionRequestDTO.Name,
                RegionImageUrl = addRegionRequestDTO.RegionImageUrl,
            };

            await _context.Regions.AddAsync(regiondomain);
            await _context.SaveChangesAsync();

            return regiondomain;

        }

        public async Task<Region?> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            var region = await _context.Regions.FirstOrDefaultAsync(x => x.Id == id);

            return region;
        }

        public async Task<Region?> Delete([FromRoute] Guid id)
        {
            var region = await _context.Regions.FirstOrDefaultAsync(x => x.Id == id);
            return region;

        }


    }
}
