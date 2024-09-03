using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    {
        private readonly NZWalksDbContext _context;
        {
        }

        [HttpGet]
        {

            {
                {
                });
            }

        }

        [HttpGet]
        {
            var region = _context.Regions.FirstOrDefault(_context => _context.Id == id);

            if (region is null) {
                return NotFound();
            }

            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl,
            };

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            // we dont need id here so thats why we are using dto or else we can directly pass region
            //map dto to domain model
            /*var regiondomainmodel = new Region
            {
                Code = addRegionRequestDTO.Code,
                Name = addRegionRequestDTO.Name,
                RegionImageUrl = addRegionRequestDTO.RegionImageUrl,
            };

             await _context.Regions.AddAsync(regiondomainmodel);
             await _context.SaveChangesAsync();

            _regionRepo.Create(addRegionRequestDTO);
*/
            var regiondomainmodel = await _regionRepo.Create(addRegionRequestDTO);

        {
            {
            };



            {

        }
    }
}
