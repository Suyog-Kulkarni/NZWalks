using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.DTO;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _context;
        public RegionsController(NZWalksDbContext zWalksDbContext)
        {
            _context = zWalksDbContext;
        }

        [HttpGet]

        public IActionResult GetAll()
        {
            // get the data from tha database - domain model
            var regions = _context.Regions.ToList();
            // map to dtos
            var regionsDto = new List<RegionDTO>();

            foreach (var region in regions)
            {
                regionsDto.Add(new RegionDTO
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl,
                });
            }

            //return
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetByID(Guid id)
        {
            // domain model
            var region = _context.Regions.FirstOrDefault(x => x.Id == id);
            if (region is null)
            {
                return NotFound();
            }

            // map it
            var regionsDto = new RegionDTO
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl,
            };

            return Ok(regionsDto);
        }

        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            //map dto to domain model
            var regiondomainmodel = new Region
            {
                Code = addRegionRequestDTO.Code,
                Name = addRegionRequestDTO.Name,
                RegionImageUrl = addRegionRequestDTO.RegionImageUrl,
            };

            _context.Regions.Add(regiondomainmodel);
            _context.SaveChanges();

            // again map domain model to dto to show to client

            var regiondto = new re
            return CreatedAtAction(nameof(GetByID), new { id = regiondomainmodel.Id });
        }

    }
}
