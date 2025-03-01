namespace NZWalks.API.Models.DTO
{
    public class RegionDTO
    {
        /// <summary>
        public Guid Id { get; set; }
        /// </summary>

        public string Name { get; set; }

        public string Code { get; set; }
        public String? RegionImageUrl { get; set; }
    }
}
