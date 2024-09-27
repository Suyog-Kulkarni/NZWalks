using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _context;
        private readonly IRegionRepo _regionRepo;
        private readonly IMapper _mapper;
        public RegionsController(NZWalksDbContext zWalksDbContext, IRegionRepo regionRepo, IMapper mapper)
        {
            _context = zWalksDbContext;
            _regionRepo = regionRepo;
            _mapper = mapper;
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            // get the data from tha database - domain model
            var regions = await _regionRepo.GetAll();
            // map to dtos
            /*var regionsDto = new List<RegionDTO>();*/

            /*foreach (var region in regions)
            {
                regionsDto.Add(new RegionDTO
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl,
                });
            }*/
            // map to dtos
            var regionsDto = _mapper.Map<List<RegionDTO>>(regions);

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
           /* var regionsDto = new RegionDTO
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl,
            };*/

            return Ok(_mapper.Map<RegionDTO>(region));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            // we dont need id here so thats why we are using dto or else we can directly pass region
            //map dto to domain model
            var regiondomainmodel = _mapper.Map<Region>(addRegionRequestDTO);

            regiondomainmodel = await _regionRepo.Create(addRegionRequestDTO);

            // again map domain model to dto to show to client what has been done

            var regiondto = _mapper.Map<RegionDTO>(regiondomainmodel);

            return CreatedAtAction(nameof(GetByID), new { Id = regiondto.Id }, regiondto);
        }

        [HttpPut]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            // map dto to model
            var regionDModel = _mapper.Map<Region>(updateRegionRequestDTO);

            regionDModel = await _regionRepo.Update(id, regionDModel);

            if (regionDModel is null)
            {
                return NotFound();
            }

            // convert domain model to dto 

            var regiondto = _mapper.Map<RegionDTO>(regionDModel);

            return Ok(regiondto);
        }
        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var region = await _regionRepo.Delete(id);

            if (region is null)
            {
                return NotFound();

            }

            var regionDto = _mapper.Map<RegionDTO>(region);

            return Ok(regionDto);
        }
    }
}