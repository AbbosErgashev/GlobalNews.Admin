namespace News.Admin.DTO.NewsDto;
#pragma warning disable
public class NewsItemDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string? MediaUrl { get; set; }
    public IFormFile? MediaFile { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int CategoryId { get; set; }
    public string Categories { get; set; }
}
