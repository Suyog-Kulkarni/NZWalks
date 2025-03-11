using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalksRepo _walksRepo;

        public WalksController(IMapper mapper, IWalksRepo walksRepo)
        {
            _mapper = mapper;
            _walksRepo = walksRepo;
        }

        [HttpGet]
        //api/walks?filterOn=Name&filterQuery=Track
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [
            FromQuery] string? sortBy, [FromQuery] bool? isAscending, 
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            var walks = await _walksRepo.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            // map to dto
            return Ok(_mapper.Map<List<WalkDTO>>(walks));
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var Walk = await _walksRepo.GetByIdAsync(id);

            if (Walk is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDTO>(Walk));
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalksDTO addWalksDTO)
        {

            //map dto to domain model
            var walkModel = _mapper.Map<Walk>(addWalksDTO);

            await _walksRepo.CreateAsync(walkModel);

            //map domain model back to dto

            return Ok(_mapper.Map<WalkDTO>(walkModel));


        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkDTO updateWalk)
        {

            // map to domain
            var WalkModel = _mapper.Map<Walk>(updateWalk);

            WalkModel = await _walksRepo.UpdateAsync(id, WalkModel);

            if (WalkModel is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDTO>(WalkModel));

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var Walk = await _walksRepo.DeleteAsync(id);

            if (Walk is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDTO>(Walk));
        }
    }
}
