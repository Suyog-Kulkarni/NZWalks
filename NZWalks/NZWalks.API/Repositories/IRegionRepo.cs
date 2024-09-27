using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepo
    {
        Task<List<Region>> GetAll();

        Task<Region?> GetById(Guid id);

        Task<Region> Create(AddRegionRequestDTO addRegionRequestDTO);

        Task<Region?> Update([FromRoute]Guid id, Region region);

        Task<Region?> Delete([FromRoute]Guid id);
    }
}
