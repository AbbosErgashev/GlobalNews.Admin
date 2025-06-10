using System.ComponentModel.DataAnnotations;

namespace News.Admin.DTO.NewsDto;

public class NewsItemCreateDto
{
    [MaxLength(100)]
    public required string Title { get; set; }
    public required string Description { get; set; }
    public IFormFile? MediaFile { get; set; }
    public int CategoryId { get; set; }
}
