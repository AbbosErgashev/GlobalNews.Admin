﻿using News.Admin.DTO.NewsDto;
using News.Admin.Models;

namespace News.Admin.NewsMapping;

#pragma warning disable
public static class NewsMapper
{
    public static NewsItemDto ToDto(NewsItem item) => new()
    {
        Id = item.Id,
        Title = item.Title,
        Description = item.Description,
        MediaUrl = item.MediaUrl,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt,
        CategoryId = item.CategoryId,
        Categories = item.Category?.Name
    };

    public static NewsItem ToEntity(NewsItemDto dto) => new()
    {
        Id = dto.Id,
        Title = dto.Title,
        Description = dto.Description,
        MediaUrl = dto.MediaUrl!,
        CreatedAt = dto.CreatedAt,
        UpdatedAt = dto.UpdatedAt,
        CategoryId = dto.CategoryId
    };
}
