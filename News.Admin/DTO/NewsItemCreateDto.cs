namespace News.Admin.DTO;

public class NewsItemCreateDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public IFormFile? MediaFile { get; set; }
}
