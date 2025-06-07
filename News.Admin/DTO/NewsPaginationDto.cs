namespace News.Admin.DTO;

public class NewsPaginationDto
{
    public List<NewsItemDto> NewsItemDtos { get; set; } = new();
    public int CurrentPage { get; set; }
    public int TotalPage { get; set; }
}
