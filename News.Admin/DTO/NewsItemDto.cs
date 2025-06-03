using Microsoft.Build.ObjectModelRemoting;

namespace News.Admin.DTO;

public class NewsItemDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string? MediaUrl { get; set; }
    public IFormFile? MediaFile { get; set; }
    public DateTime CreatedAt { get; set; }
}
