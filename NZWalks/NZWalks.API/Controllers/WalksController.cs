using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public WalksController(IMapper mapper, IWalksRepo walksRepo) {
            _mapper = mapper;
            _walksRepo = walksRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walks = await _walksRepo.GetAllAsync();

            // map to dto
            return Ok(_mapper.Map<List<WalkDTO>>(walks));
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var Walk = await _walksRepo.GetByIdAsync(id);

            if(Walk is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDTO>(Walk));
        }

        [HttpPost]
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
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkDTO updateWalk)
        {
            // map to domain
            var WalkModel = _mapper.Map<Walk>(updateWalk);

            WalkModel = await _walksRepo.UpdateAsync(id, WalkModel);

            if(WalkModel is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDTO>(WalkModel));
        }
    }
}
