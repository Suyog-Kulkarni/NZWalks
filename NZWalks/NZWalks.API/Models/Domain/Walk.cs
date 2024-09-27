using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.API.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        // these are foregin key which will be mapped with the respective model
        //[ForeignKey("Region")]

        // ef core automatically detects a foreign key based on the name and their respective navigation property
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }

        // Navigation Propertiee
        public Difficulty Difficulty { get; set; }

        public Region Region { get; set; }


    }
}
