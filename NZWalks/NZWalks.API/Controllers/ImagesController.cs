using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDTO requestDTO)
        {
            ValidateFile(requestDTO);

            if (ModelState.IsValid)
            {
                // convert the request to domain model because repo works with domain model

                var imageDomainModel = new Image
                {
                    File = requestDTO.File,
                    FileName = requestDTO.FileName,
                    FileDescription = requestDTO.FileDescription,
                    FileExtension = Path.GetExtension(requestDTO.File.FileName),
                    FileSizeInBytes = requestDTO.File.Length,

                };

                await _imageRepository.Upload(imageDomainModel);

                return Ok(imageDomainModel);

            }
            return BadRequest(ModelState);
        }
        private void ValidateFile(ImageUploadRequestDTO request)
        {
            // Check if the file is not null
            if (request.File == null || request.File.Length == 0)
            {
                ModelState.AddModelError("File", "File is required.");
            }
            // Check if the file size is within the limit (e.g., 5MB)
            if (request.File.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("File", "File size exceeds the limit of 5MB.");
            }
            // Check if the file type is allowed (e.g., image files)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(request.File.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("File", "Invalid file type. Only image files are allowed.");
            }
        }

    }
}
