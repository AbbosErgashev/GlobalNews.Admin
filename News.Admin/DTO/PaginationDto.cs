namespace News.Admin.DTO;

public class PaginationDto
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? SearchText { get; set; }
    public string? SortOrder { get; set; }
    public int? CategoryId { get; set; }
}
