using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.DTO;
using NZWalks.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _context;
        private readonly IRegionRepo _regionRepo;
        public RegionsController(NZWalksDbContext zWalksDbContext, IRegionRepo regionRepo)
        {
            _context = zWalksDbContext;
            _regionRepo = regionRepo;
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            // get the data from tha database - domain model
            var regions = await _regionRepo.GetAll();
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
        public async Task<IActionResult> GetByID(Guid id)
        {
            // domain model
            var region = await _regionRepo.GetById(id);
            if (region is null)
            {
                return NotFound();
            }

            // map it to dto 
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

            // again map domain model to dto to show to client what has been done

            var regiondto = new RegionDTO
            {
                Id = regiondomainmodel.Id,
                Code = regiondomainmodel.Code,
                Name = regiondomainmodel.Name,
                RegionImageUrl = regiondomainmodel.RegionImageUrl,
            };
            return CreatedAtAction(nameof(GetByID), new {Id = regiondto.Id}, regiondto);
        }

        [HttpPut]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            var regionDModel = await _regionRepo.Update(id, updateRegionRequestDTO);

            if(regionDModel is null)
            {
                return NotFound();
            }

            // map dto to domain model
            regionDModel.RegionImageUrl = updateRegionRequestDTO.RegionImageUrl;
            regionDModel.Name = updateRegionRequestDTO.Name;
            regionDModel.Code = updateRegionRequestDTO.Code;

            //_context.Regions.Update(regionDModel);
            await _context.SaveChangesAsync();

            // convert domain model to dto 

            var regiondto = new RegionDTO
            {
                Id = regionDModel.Id,
                Code = regionDModel.Code,
                Name = regionDModel.Name,
                RegionImageUrl = regionDModel.RegionImageUrl,
            };

            return Ok(regiondto);
        }
        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var region = await _regionRepo.Delete(id);

            if(region is null)
            {
                return NotFound();
                
            }
             _context.Regions.Remove(region);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message="Resource Deleted",
                DeletedResource = new RegionDTO
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl,
                }
            });
        }
    }
}
