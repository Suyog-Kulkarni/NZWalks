using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddWalksDTO
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Description { get; set; }
        [Required]
        [Range(1,50, ErrorMessage ="Range Error")]
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set; }
    }
}
