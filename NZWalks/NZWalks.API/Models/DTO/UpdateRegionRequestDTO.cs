using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDTO
    {

        [Required]
        [MaxLength(100, ErrorMessage = "Not accepted")]
        public string Name { get; set; }

        [Required]
        [MaxLength(3, ErrorMessage = "Invalid Code")]
        [MinLength(3, ErrorMessage = "Invalid Code")]
        public string Code { get; set; }
        public String? RegionImageUrl { get; set; }
    }
}
