using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using System.Runtime.CompilerServices;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : Controller
    {
        private readonly NZWalksDbContext _context;
        public RegionsController(NZWalksDbContext nZWalksDbContext) 
        {
            _context = nZWalksDbContext;
        }

        [HttpGet]
        public IActionResult GeatAll()
        {
            // Get data from the database - Domain Model
            var regions = _context.Regions.ToList();

            // Map to Model to DtO
            var Regiondto = new List<RegionDTO>();
            for(int i = 0; i < regions.Count; i++)
            {
                Regiondto.Add(new RegionDTO
                {
                   Id = regions[i].Id,
                   Code = regions[i].Code,
                   Name = regions[i].Name,
                   RegionImageUrl = regions[i].RegionImageUrl,
                });
            }

            //return DTO
            return Ok(Regiondto);
        }

        [HttpGet]
        [Route("{id:guid}")]

        public IActionResult GetById([FromRoute] Guid id) 
        {
            var region = _context.Regions.FirstOrDefault(_context => _context.Id == id);

            if (region is null) {
                return NotFound();
            }

            var regionDTO = new RegionDTO
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl,
            };

            return Ok(regionDTO);
        }

        [HttpPost]

        public IActionResult CreateRegion([FromBody] AddRegionDTO addRegionDTO)
        {
            // map to domain model
            var domainmodel = new Region
            {
                Code = addRegionDTO.Code,
                Name = addRegionDTO.Name,
                RegionImageUrl = addRegionDTO.RegionImageUrl,
            };

            _context.Regions.Add(domainmodel);
            _context.SaveChanges();

            // map again to dto to show to client that the region has been created

            var regiondto = new AddRegionDTO
            {
                Code = domainmodel.Code,
                Name = domainmodel.Name,
                RegionImageUrl = domainmodel.RegionImageUrl,
            };

            return CreatedAtAction(nameof(GetById), regiondto);
        }
    }
}
