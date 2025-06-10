namespace News.Admin.DTO.NewsDto;

public class NewsPaginationDto
{
    public List<NewsItemDto> NewsItemDtos { get; set; } = [];
    public int CurrentPage { get; set; }
    public int TotalPage { get; set; }
    public string? SearchText { get; set; }
    public string? SortOrder { get; set; }
    public int? CategoryId { get; set; }
}
