using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.API.Models.Domain;
public class Image
{
    public Guid Id { get; set; }

    [NotMapped]
    public IFormFile File { get; set; } // this property is used to upload the image file from the client side to the server side

    public string FileName { get; set; }

    public string? FileDescription { get; set; }
    public string FileExtension { get; set; }
    public long FileSizeInBytes { get; set; }
    public string FilePath { get; set; }

}

