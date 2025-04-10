using NZWalks.UI.Models.Domain; // Assuming Difficulty and Region models are here
using System.ComponentModel.DataAnnotations;

namespace NZWalks.UI.Models.DTO
{
    // This model will be used by the Add Walk view form
    public class AddWalkViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(0, 50)]
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set; }

        // You might need collections to populate dropdowns in the view
        // These would typically be fetched from the API in the GET Add action
        // public IEnumerable<Difficulty> Difficulties { get; set; }
        // public IEnumerable<Region> Regions { get; set; }
    }
} 