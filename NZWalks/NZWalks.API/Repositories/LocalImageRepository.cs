using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly NZWalksDbContext _dbContext;
        public LocalImageRepository(IWebHostEnvironment webHost, IHttpContextAccessor httpContextAccessor, NZWalksDbContext context)
        {
            _webHost = webHost;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = context;
        }
        public async Task<Image> Upload(Image image)
        {
            var localPath = Path.Combine(_webHost.ContentRootPath, "Images",$"{image.FileName}{image.FileExtension}");

            using var stream = new FileStream(localPath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var imagePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = imagePath; 
            await _dbContext.Images.AddAsync(image);
            await _dbContext.SaveChangesAsync();

            return image;   

        }
    }
}
