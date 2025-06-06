using News.Admin.DTO;
using News.Admin.Models;

namespace News.Admin.NewsMapping;

public static class NewsMapper
{
    public static NewsItemDto ToDto(NewsItem item) => new()
    {
        Id = item.Id,
        Title = item.Title,
        Description = item.Description,
        MediaUrl = item.MediaUrl,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt
    };

    public static NewsItem ToEntity(NewsItemDto dto) => new()
    {
        Id = dto.Id,
        Title = dto.Title,
        Description = dto.Description,
        MediaUrl = dto.MediaUrl!,
        CreatedAt = dto.CreatedAt,
        UpdatedAt = dto.UpdatedAt
    };
}
